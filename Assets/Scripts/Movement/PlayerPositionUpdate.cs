using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PMFlags
{
    PMF_DUCKED = 1,
    PMF_JUMP_HELD = 2,
    PMF_ON_GROUND = 4,
    PMF_TIME_WATERJUMP = 8
}


public class PlayerPositionUpdate : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    private Rigidbody rb;
    private InputMaster controls;
    private Vector2 currentMovement;
    [SerializeField] private CameraFollow Camera;


    public AudioSource jumpSoundSource;

    //collider
    public CapsuleCollider colliderObject;

    //move state
    public float viewheight;
    private PMFlags pmflags;

    private Vector3 new_position = Vector3.zero;
    private Vector3 new_velocity = Vector3.zero;
    private Vector3 position = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private Vector3 previous_origin = Vector3.zero;
    private Vector3 previous_camera_position = Vector3.zero;

    //for fixed update
    private PMFlags asyncFlags;

    private Vector3 lastAsyncOrigin;
    private Vector3 lastAsyncVelocity;
    private Vector3 lastAsyncAddVelocity = Vector3.zero;
    private float elapsedAsyncFrametime;

    private void Awake()
    {
        // subscribe lambda functions to the performed event of the actions in "InputMaster"
        controls = new InputMaster();
        controls.Player.Move.performed += ctx => currentMovement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => currentMovement = Vector2.zero;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void FixedUpdate()
    {
        MoveData movedata = new MoveData 
        {
            oldPosition = lastAsyncOrigin,
            oldVelocity = lastAsyncVelocity,

            oldForward = Camera.transform.forward,
            oldRight = Camera.transform.right,

            addVelocities = lastAsyncAddVelocity,

            gravity = PlayerState.pm_gravity,
            frametime = Time.fixedDeltaTime,
            flags = asyncFlags
        };

        // execute player movement functions
        PlayerMovement.DoMove(movedata, currentMovement);

        lastAsyncOrigin = movedata.newPosition;
        lastAsyncVelocity = movedata.newVelocity;
        lastAsyncAddVelocity = Vector3.zero;
        asyncFlags = movedata.flags;

        // restart frametime counter
        elapsedAsyncFrametime = 0;

        //overwrite all sync variables with async frame
        position = movedata.newPosition * 0.125f;
        new_position = movedata.newPosition;
        new_velocity = movedata.newVelocity;
        velocity = movedata.newVelocity * 0.125f;
        pmflags = movedata.flags;
        viewheight = movedata.viewheight;

        //update PlayerState
        PlayerState.currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        PlayerState.currentViewHeight = viewheight;
        PlayerState.currentPosition = position;

        PlayerState.mins = movedata.mins;
        PlayerState.maxs = movedata.maxs;


        // Strafe movement
        //Vector3 movement = new Vector3(currentMovement.x, 0, currentMovement.y);
        //Vector3 newPosition = rb.position + transform.TransformDirection(movement) * speed * Time.fixedDeltaTime;
        //rb.MovePosition(newPosition);


    }
    void Update()
    {
        Camera.DoLook();

        MoveData movedata;

        elapsedAsyncFrametime += Time.deltaTime;

        movedata = new MoveData
        {
            oldPosition = lastAsyncOrigin,
            oldVelocity = lastAsyncVelocity,

            oldForward = Camera.transform.forward,
            oldRight = Camera.transform.right,

            addVelocities = PlayerState.addVelocities,
            gravity = PlayerState.pm_gravity,
            frametime = elapsedAsyncFrametime, //run delta time from last async frame
            flags = pmflags
        };

        //queue velocities for async
        lastAsyncAddVelocity += PlayerState.addVelocities;
        PlayerState.addVelocities = Vector3.zero;

        // execute player movement functions
        PlayerMovement.DoMove(movedata, currentMovement);

        //play jump sound
        if (movedata.jumped)
        {
            jumpSoundSource.Play();
        }

        //set data
        position = movedata.newPosition * 0.125f;
        velocity = movedata.newVelocity * 0.125f;

        pmflags = movedata.flags;
        viewheight = movedata.viewheight;

        //update camera
        //ApplyPmoveToCamera();

        //update player state info
        PlayerState.currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        PlayerState.currentViewHeight = viewheight;
        PlayerState.currentPosition = position;

        PlayerState.mins = movedata.mins;
        PlayerState.maxs = movedata.maxs;

        //snap position and store old origin
        new_position = position * 8;
        new_velocity = movedata.newVelocity;


        //Finally... Update Player Game position from PlayerState
        rb.MovePosition(PlayerState.currentPosition);
    }

    //bool isLerping = false;
    //private void ApplyPmoveToCamera()
    //{
    //    float scale = Globals.scale.Value;
    //    Vector3 newCamPos = new Vector3(origin.x, origin.y, origin.z) + Vector3.up * viewheight;

    //    //smooth out stepping up and crouching
    //    float step = newCamPos.y - previous_camera_position.y;

    //    if (!pmflags.HasFlag(PMFlags.PMF_ON_GROUND) || step == 0)
    //    {
    //        isLerping = false;
    //    }
    //    else
    //    {
    //        isLerping = true;
    //    }

    //    if (isLerping)
    //    {
    //        newCamPos.y = Mathf.Lerp(previous_camera_position.y, newCamPos.y, (pmflags.HasFlag(PMFlags.PMF_DUCKED) ? 25f : 12.5f) * Time.deltaTime);
    //    }

    //    previous_camera_position = newCamPos;
    //    Camera.transform.position = new Vector3(newCamPos.x, newCamPos.y, newCamPos.z) * scale;

    //    //set collider
    //    colliderObject.gameObject.transform.position = Camera.transform.position - Vector3.up * viewheight * scale;

    //    //please unity, just let me set collider bounds...
    //    Bounds b = new Bounds();
    //    b.SetMinMax(PlayerState.mins * scale, PlayerState.maxs * scale);

    //    colliderObject.center = b.center;
    //    colliderObject.size = b.extents * 2;
    //}

}