using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Instructions:
 *  - None
 * 
 * Description:
 *  - This script is a collection of movement functions used by the PlayerPositionUpdate script. These functions are heavily modified but mostly
 *  based on Maciej Szybiak's quake 2 unity project PlayerMove script, which is itself based on John Carmacks original quake 2 open source code.
 * 
 */


public struct MoveData
{
    //in
    public Vector3 oldPosition;
    public Vector3 oldVelocity;
    public Vector3 oldForward;
    public Vector3 oldRight;
    public Vector3 addVelocities;
    public float frametime;
    public float gravity;

    //public bool initialSnap;

    //out
    public Vector3 newPosition;
    public Vector3 newVelocity;
    public float viewheight;
    public PMFlags flags;
    public bool jumped;
    public bool beginCameraLerp;
}

public static class PlayerMovement
{
    //private const float speed = 10.0f;
    //private const float jumpForce = 5.0f;

    //constants
    private const float pm_friction = 6f;
    private const float pm_maxspeed = 300f;
    private const float pm_speedmult = 1f;
    private const float pm_stopspeed = 100f;
    private const float pm_accelerate = 10f;
    private const float pm_jumpstrength = 370f;
    private const float pm_scaling_factor = 30f;    // Scales down player movement speed. Increase this value to make all player movement slower.

    private static float timer = 0;

    private static float viewheight;
    private static PMFlags pmflags;
    private static bool groundentity = false;
    private static float frametime;
    private static MoveData movedata;
    private static Vector3 position;
    private static Vector3 velocity;

    private static void FinishPmove(BoxCollider playerCollider)
    {
        if (PM_GoodPosition(position, playerCollider))
        {
            movedata.newPosition = position;
            movedata.newVelocity = velocity;
            movedata.flags = pmflags;
            movedata.viewheight = viewheight;
        }
        else
        {
            movedata.newPosition = movedata.oldPosition;
            //movedata.newVelocity = Vector3.zero;
        }
        
    }

    public static void DoMove(ref MoveData movedata, Vector2 currentMovement, BoxCollider playerCollider)
    {
        movedata.viewheight = 2;
        PlayerMovement.movedata = movedata;

        //Debug.Log("Initial position: " + movedata.oldPosition);
        //Debug.Log("Initial velocity: " + movedata.oldVelocity);

        position = movedata.oldPosition;
        velocity = movedata.oldVelocity;
        frametime = movedata.frametime;
        pmflags = movedata.flags;

        if (movedata.addVelocities != Vector3.zero)
        {
            velocity += movedata.addVelocities;
        }

        //PM_CheckDuck();

        PM_CategorizePosition(playerCollider);
        //PM_CheckSpecialMovement();

        PM_CheckJump();
        PM_Friction();
        PM_AirMove(playerCollider, currentMovement);
        PM_CategorizePosition(playerCollider);
        //Debug.Log("After move functions: position: " + position + " velocity:" + velocity);

        PM_GoodPosition(position, playerCollider);

        FinishPmove(playerCollider);
        movedata = PlayerMovement.movedata;
        //Debug.Log("--------------End----------------");
    }


    /// <summary>
    /// Checks whether player is stuck in a wall.
    /// </summary>
    private static bool PM_GoodPosition(Vector3 pos, BoxCollider playerCollider)
    {
        int layerMask = 1 << LayerMask.NameToLayer("LevelStructure");

        bool isValid = !Physics.CheckBox(pos, playerCollider.bounds.extents/1.5f, Quaternion.identity, layerMask);
        //Debug.Log("PM_GoodPosition - isValid: " + isValid);

        return isValid;

        // Use Unity's Physics.CheckBox() to check if the position is valid.
        // The extents of the box are assumed to be already set in the player's collider.
        //return !Physics.CheckBox(pos, playerCollider.bounds.extents, Quaternion.identity, layerMask);
    }

    private static void PM_CategorizePosition(BoxCollider playerCollider)
    {
        RaycastHit hit;
        float distanceToGround = playerCollider.bounds.extents.y;

        // Check if player is falling
        if (velocity.y > 180)
        {
            //Debug.Log("velocity.y >180, player is falling");
            groundentity = false;
            pmflags &= ~PMFlags.PMF_ON_GROUND;
        }
        else
        {
            // Raycast downwards to check for ground
            if (Physics.Raycast(position, Vector3.down, out hit, distanceToGround + 0.05f))
            {
                //Debug.Log("Raycast hit something, checking if it's ground");

                if (hit.normal.y < 0.7f)
                {
                    Debug.Log("Surface is too steep to be ground");
                    groundentity = false;
                    pmflags &= ~PMFlags.PMF_ON_GROUND;
                }
                else
                {
                    //Debug.Log("Player is on the ground");
                    groundentity = true;
                    if (!pmflags.HasFlag(PMFlags.PMF_ON_GROUND))
                    {
                        // just hit the ground
                        pmflags |= PMFlags.PMF_ON_GROUND;
                    }
                }
            }
            else
            {
                //Debug.Log("Raycast didn't hit anything, player is in the air");
                groundentity = true;
                pmflags &= ~PMFlags.PMF_ON_GROUND;
            }
        }
    }
    private static void PM_CheckJump()
    {
        if (!Input.GetKeyDown(KeyCode.Space))  // TODO: Replace with proper input condition
        {
            pmflags &= ~PMFlags.PMF_JUMP_HELD;
            return;
        }

        // If jump was already pressed, just return
        if (pmflags.HasFlag(PMFlags.PMF_JUMP_HELD))
        {
            return;
        }

        // Check if on the ground using flags instead of a separate bool
        if (!pmflags.HasFlag(PMFlags.PMF_ON_GROUND))        // TODO: was if(!groundEntity), fixed jumping in the air but now cant jump on ramps, add an extra flag for ramps in SSM?
        {
            return;
        }

        pmflags |= PMFlags.PMF_JUMP_HELD;

        // Mark as no longer on the ground
        groundentity = false;
        pmflags &= ~PMFlags.PMF_ON_GROUND;

        velocity.y += pm_jumpstrength;
        if (velocity.y < pm_jumpstrength)
        {
            velocity.y = pm_jumpstrength;
        }

        // Flag to play a jump sound or something similar
        movedata.jumped = true;
    }

    /// <summary>
    /// Apply ground (TODO: and water) friction.
    /// </summary>
    private static void PM_Friction()
    {
        float speed, newspeed, control;
        float friction;
        float drop;

        speed = velocity.magnitude;

        if (speed < (1/pm_scaling_factor))   // TODO: Scale this properly?
        {
            velocity.x = 0;
            velocity.z = 0;
            return;
        }

        drop = 0;

        //apply ground friction
        if (groundentity && ((int)PMFlags.PMF_ON_GROUND != 0))
        {
            friction = pm_friction;
            control = speed < pm_stopspeed ? pm_stopspeed : speed;
            drop += control * friction * frametime;
        }

        //scale the velocity
        newspeed = speed - drop;
        if (newspeed < 0)
        {
            newspeed = 0;
        }
        newspeed /= speed;
        velocity *= newspeed;
    }

    private static void PM_AirMove(BoxCollider playerCollider, Vector2 currentMovement)
    {
        //Debug.Log("performing move function");
        Vector3 wishvel = new Vector3();
        float fmove, smove;
        Vector3 wishdir;
        Vector3 flatforward, flatright;
        float wishspeed;
        float maxspeed;

        fmove = currentMovement.y * 300f;
        //Debug.Log("fmove:" + fmove);
        smove = currentMovement.x * 300f;
        //Debug.Log("smove:" + smove);

        flatforward = movedata.oldForward;
        flatforward.y = 0;
        flatforward.Normalize();
        //Debug.Log("flatforward:" + flatforward);

        flatright = movedata.oldRight;
        flatright.y = 0;
        flatright.Normalize();
        //Debug.Log("flatright:" + flatright);

        wishvel.x = flatforward.x * fmove + flatright.x * smove;
        //Debug.Log("wishvel x:" + wishvel.x);
        wishvel.z = flatforward.z * fmove + flatright.z * smove;
        //Debug.Log("wishvel z:" + wishvel.z);
        wishvel.y = 0;

        //PM_AddCurrents(ref wishvel); // Accounts for ladders

        wishdir = new Vector3(wishvel.x, wishvel.y, wishvel.z);
        wishspeed = wishdir.magnitude;
        //Debug.Log("wishspeed:" + wishspeed);
        wishdir.Normalize();
        //Debug.Log("wishdir:" + wishdir);


        //clamp to maxspeed
        //maxspeed = pmflags.HasFlag(PMFlags.PMF_DUCKED) ? pm_duckspeed : pm_maxspeed;
        maxspeed = pm_maxspeed;
        //Debug.Log("maxspeed:" + maxspeed);

        if (wishspeed > maxspeed)
        {
            wishvel *= maxspeed / wishspeed;
            wishspeed = maxspeed;
        }

        if ((pmflags & PMFlags.PMF_ON_GROUND) != 0)
        {
            //walking on ground
            velocity.y = 0;
            PM_Accelerate(wishdir, wishspeed, pm_accelerate);
            //Debug.Log("performing move on ground with velocity:" + velocity);

            //fix for negative gravity here      
            PM_StepSlideMove(playerCollider);
        }
        else
        {
            PM_Accelerate(wishdir, wishspeed, 1);
            //Debug.Log("performing move in air");
            //add gravity
            velocity.y -= PlayerState.pm_gravity * frametime;

            PM_StepSlideMove(playerCollider);
        }
    }

    private static void PM_Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed, accelspeed, currentspeed;

        currentspeed = Vector3.Dot(velocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }
        accelspeed = accel * frametime * wishspeed;

        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        velocity += accelspeed * wishdir;
        //Debug.Log("after acceleration velocity:" + velocity);
    }

    /// <summary>
    /// Moves the player along velocity vector and try to step up.
    /// </summary>
    private static void PM_StepSlideMove(BoxCollider playerCollider)
    {
        Vector3 start_o, start_v;
        Vector3 down_o, down_v;
        RaycastHit hitInfo; // Using RaycastHit instead of TraceT
        float down_dist, up_dist;
        Vector3 up, down;
        int layerMask = 1 << LayerMask.NameToLayer("LevelStructure");

        //Debug.Log("StepSlide initial position:" + position);
        start_o = position;
        start_v = velocity;

        PM_StepSlideMove_(playerCollider);
        //Debug.Log("Position after PM_StepSlideMove_1: " + position);


        down_o = position;
        down_v = velocity;

        //Debug.Log("Position before step up: " + position);

        up = start_o;
        up += new Vector3(0, 1f, 0);

        // Using BoxCast instead of custom Trace
        if (Physics.BoxCast(up, playerCollider.size / 2, Vector3.up, out hitInfo, Quaternion.identity, 1, layerMask))
        {
            Debug.Log("can't step up");
            return; //can't step up
        }

        position = up;
        //Debug.Log("Position after step up: " + position);
        velocity = start_v;

        PM_StepSlideMove_(playerCollider); 
        //Debug.Log("Position after PM_StepSlideMove_2(should be forward AND up): " + position);


        //push down the final amount
        down = position;
        down.y -= 1f;
        if (Physics.BoxCast(position, playerCollider.bounds.extents, Vector3.down, out hitInfo, Quaternion.identity, 1f, layerMask))
        {
            //Debug.Log("Setting position to Boxcast hit" + hitInfo.point + hitInfo.normal);
            position.y = hitInfo.point.y + 1;
            Debug.Log("Position after Boxcast hit:" + position);
        }

        up = position;

        down_dist = Mathf.Pow(down_o.x - start_o.x, 2) + Mathf.Pow(down_o.z - start_o.z, 2);
        up_dist = Mathf.Pow(up.x - start_o.x, 2) + Mathf.Pow(up.z - start_o.z, 2);

        if (down_dist > up_dist || hitInfo.normal.y < 0.7f)
        {
            position = down_o;
            //Debug.Log("StepSlide taking down position:" + position);
            velocity = down_v;
            return;
        }

        //special case blah blah
        velocity.y = down_v.y;

        //movedata.beginCameraLerp = true;
    }


    private static void PM_StepSlideMove_(BoxCollider playerCollider)
    {
        //Debug.Log("Entered PM_SSM_");
        int bumpcount, numbumps;
        Vector3 dir;
        float d;
        int numplanes;
        Vector3[] planes = new Vector3[5];
        Vector3 primal_velocity;
        int i, j;
        RaycastHit hitInfo; // Replaces TraceT trace
        Vector3 end;
        float time_left;
        int layerMask = 1 << LayerMask.NameToLayer("LevelStructure");

        numbumps = 4;
        primal_velocity = velocity;
        numplanes = 0;
        time_left = frametime;

        for (bumpcount = 0; bumpcount < numbumps; bumpcount++)
        {
            end = position + time_left * velocity/pm_scaling_factor;

            // Using Unity's BoxCast to replace CL_Trace
            bool hit = Physics.BoxCast(position, playerCollider.size / 2, velocity, out hitInfo, playerCollider.transform.rotation, Vector3.Distance(position, end), layerMask);

            //if (hit)
            //{
            //    Debug.Log("Collision detected at: " + hitInfo.point);
            //    Debug.Log("Hit normal: " + hitInfo.normal);
            //}
            //if (hitInfo.distance > 0)
            //{
            //    Debug.Log("New position: " + position);
            //    Debug.Log("New velocity: " + velocity);
            //}

            if (!hit)
            {
                //Debug.Log("PM_SSM_ Boxcast didn't hit");
                position = end;
                break;  // Equivalent to if (trace.fraction == 1)
            }

            if (hitInfo.collider != null)
            {
                //Debug.Log("PM_SSM_ Boxcast hit");
                // Entity is trapped in another solid
                if (hitInfo.distance == 0)
                {
                    Debug.Log("PM_SSM_ Boxcast ran straight into a solid object");
                    velocity.y = 0;
                    return;
                }

                // Actually covered some distance
                if (hitInfo.distance > 0)
                {
                    Debug.Log("PM_SSM_ hit a solid object after" + hitInfo.distance);
                    position = hitInfo.point;
                    numplanes = 0;
                }

                // Entity touch logic (if needed)

                time_left -= time_left * hitInfo.distance / Vector3.Distance(position, end);

                if (numplanes >= 5)
                {
                    // This shouldn't really happen
                    velocity = Vector3.zero;
                    break;
                }

                for (i = 0; i < numplanes; i++)
                {
                    if (planes[i] != null && Vector3.Dot(hitInfo.normal, planes[i]) > 0.99f)
                    {
                        velocity += hitInfo.normal;
                        break;
                    }
                }

                if (i < numplanes)
                {
                    continue;
                }

                planes[numplanes] = hitInfo.normal;
                numplanes++;

                // Modify original velocity so it parallels all of the clip planes
                for (i = 0; i < numplanes; i++)
                {
                    PM_ClipVelocity(velocity, planes[i], out velocity, 1.01f);

                    for (j = 0; j < numplanes; j++)
                    {
                        if (j != i)
                        {
                            if (Vector3.Dot(velocity, planes[j]) < 0)
                            {
                                break; // Not OK
                            }
                        }
                    }

                    if (j == numplanes)
                    {
                        break;
                    }
                }

                if (i != numplanes)
                {
                    Debug.Log("Going along this plane");
                    // Go along this plane
                }
                else
                {
                    Debug.Log("Going along the crease");
                    // Go along the crease
                    if (numplanes != 2)
                    {
                        velocity = Vector3.zero;
                        break;
                    }

                    dir = Vector3.Cross(planes[0], planes[1]);
                    d = Vector3.Dot(dir, velocity);
                    velocity = dir * d;
                }

                if (Vector3.Dot(velocity, primal_velocity) <= 0)
                {
                    velocity = Vector3.zero;
                    break;
                }
            }
        }
    }


    private static void PM_ClipVelocity(Vector3 input, Vector3 normal, out Vector3 output, float overbounce)
    {
        float backoff;
        float change;

        backoff = Vector3.Dot(input, normal);

        if (backoff < 0)
        {
            backoff *= overbounce;
        }
        else
        {
            backoff /= overbounce;
        }

        Vector3 ot = new Vector3();

        for (int i = 0; i < 3; i++)
        {
            change = normal[i] * backoff;
            ot[i] = input[i] - change;
        }

        output = ot;
    }

}
