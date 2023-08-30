using UnityEngine;
using UnityEngine.InputSystem;

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
    public Vector3 mins;
    public Vector3 maxs;

    public float viewheight;

    public PMFlags flags;

    public bool jumped;
    public bool beginCameraLerp;
}

public static class PlayerMovement
{
    //private const float speed = 10.0f;
    //private const float jumpForce = 5.0f;


    public static void DoMove(MoveData movedata, Vector2 currentMovement)
    {
        // Strafe movement
        Vector3 movement = new Vector3(currentMovement.x, 0, currentMovement.y);
        movedata.newPosition = movedata.oldPosition + movement * 10.0f * Time.fixedDeltaTime;


        // Jumping

        // Ducking
    }
}
