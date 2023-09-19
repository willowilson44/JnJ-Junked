using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Instructions:
 *  - Attach to the player gameobject and assign the "Camera" reference to the main camera in inspector
 * 
 * Description:
 *  - This script is the core script needed to attach to the player. It calls movement functions to update 
 *  the player position and also links to camera, controls and playerstate.
 *  
 */

public enum PMFlags
{
    PMF_DUCKED = 1,
    PMF_JUMP_HELD = 2,
    PMF_ON_GROUND = 4,
    PMF_ON_RAMP = 8
}


public class PlayerPositionUpdate : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    //private Rigidbody rb;
    private InputMaster controls;
    private Vector2 currentMovement;
    [SerializeField] private CameraFollow Camera;
    private float y;


    public AudioSource jumpSoundSource;
    public BoxCollider playerCollider;

    //move state
    private float viewheight;
    public PMFlags pmflags;
    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    private Vector3 previous_origin = Vector3.zero;
    //private Vector3 previous_camera_position = Vector3.zero;

    //for fixed update
    private PMFlags asyncFlags;
    private Vector3 lastAsyncOrigin;
    private Vector3 lastAsyncVelocity;
    private Vector3 lastAsyncAddVelocity = Vector3.zero;

    private bool jumped;


    //Debug Variables;
    public float currentSpeed;
    //public float currentViewHeight;
    public Vector3 currentPosition;
    public float pm_gravity;
    public bool onGround;
    public bool jumpHeld;


    private void Awake()
    {
        // subscribe lambda functions to the performed event of the actions in "InputMaster"
        controls = new InputMaster();
        controls.Player.Move.performed += ctx => currentMovement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => currentMovement = Vector2.zero;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;

        // Set the target frame rate to 60 fps
        Application.targetFrameRate = 100;
        Time.fixedDeltaTime = 0.005f;

        //rb = GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //set initial positions
        position = transform.position;
        lastAsyncOrigin = transform.position;
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
        PlayerMovement.DoMove(ref movedata, currentMovement, playerCollider);

        lastAsyncOrigin = movedata.newPosition;
        lastAsyncVelocity = movedata.newVelocity;
        lastAsyncAddVelocity = Vector3.zero;
        asyncFlags = movedata.flags;

        //overwrite all sync variables with async frame
        position = movedata.newPosition;
        velocity = movedata.newVelocity;
        pmflags = movedata.flags;
        viewheight = movedata.viewheight;

        //update PlayerState
        PlayerState.currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        PlayerState.currentViewHeight = viewheight;
        PlayerState.currentPosition = position;

        if (movedata.jumped)
        {
            jumped = true;
        }

        transform.position = PlayerState.currentPosition;
        //rb.MovePosition(PlayerState.currentPosition);

        // Fetch debug variables
        currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        //currentViewHeight = PlayerState.currentViewHeight;
        currentPosition = position;
        pm_gravity = PlayerState.pm_gravity;
        jumpHeld = pmflags.HasFlag(PMFlags.PMF_JUMP_HELD);
        onGround = pmflags.HasFlag(PMFlags.PMF_ON_GROUND);


    }
    void Update()
    {
        y += Camera.DoLook();
        //Debug.Log(y);
        //Debug.Log(Camera.yRotation);
        Quaternion newRotation = Quaternion.Euler(0, y, 0);
        transform.rotation = newRotation;

        //update player state info
        PlayerState.currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        PlayerState.currentViewHeight = viewheight;
        PlayerState.currentPosition = position;

        //rb.MoveRotation(Camera.transform.rotation);

        if (jumped)
        {
            jumpSoundSource.Play();
            jumped = false;
        }

        // Fetch debug variables
    }
}