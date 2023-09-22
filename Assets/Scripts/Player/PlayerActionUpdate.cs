using System.Collections;
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


public class PlayerActionUpdate : MonoBehaviour
{
    private InputMaster controls;
    private Vector2 currentMovement;
    [SerializeField] private Camera mainCamera;
    private CameraFollow Camera;
    private float y;

    private AudioSource audioSource;
    public AudioClip[] shootSound = new AudioClip[2];
    public AudioClip jumpSound;
    public BoxCollider playerCollider;
    private Light light;

    // Move states
    private float viewheight;
    public PMFlags pmflags;
    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public Vector3 lastAsyncAddVelocity = Vector3.zero;

    // Jump and Shoot states
    public bool jumping; 
    public bool shooting;


    // to play jump sound after jumping
    private bool jumped;


    //Debug Variables;
    public float currentSpeed;
    public Vector3 currentPosition;
    public float pm_gravity;
    public bool onGround;
    //public float currentViewHeight;


    private void Awake()
    {
        // subscribe lambda functions to the performed event of the actions in "InputMaster"
        controls = new InputMaster();
        controls.Player.Move.performed += ctx => currentMovement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => currentMovement = Vector2.zero;
        controls.Player.Jump.performed += _ => {
            if (PlayerState.canJump)
            {
                jumping = true;
            }
        };
        controls.Player.Jump.canceled += _ => jumping = false;
        controls.Player.Shoot.performed += _ => {
            if (PlayerState.canShoot)
            {
                shooting = true;
            }
        };
        controls.Player.Shoot.canceled += _ => shooting = false;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If not, add an AudioSource component to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Camera = mainCamera.GetComponent<CameraFollow>();
        light = GetComponent<Light>();
        light.enabled = false;
        QualitySettings.vSyncCount = 0;

        // Set the target frame rate to 60 fps
        Application.targetFrameRate = 100;
        Time.fixedDeltaTime = 0.005f;

        //set initial positions
        position = transform.position;
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
            oldPosition = position,
            oldVelocity = velocity,
            oldForward = Camera.transform.forward,
            oldRight = Camera.transform.right,
            addVelocities = lastAsyncAddVelocity,
            gravity = PlayerState.pm_gravity,
            frametime = Time.fixedDeltaTime,
            flags = pmflags,
            jumping = this.jumping
        };

        // execute player movement functions
        PlayerMovement.DoMove(ref movedata, currentMovement, playerCollider);

        if (shooting)
        {
            PlayerShooting.DoShoot(this.gameObject, mainCamera, this);
        }

        //overwrite all sync variables with DoMove() results
        lastAsyncAddVelocity = Vector3.zero;
        position = movedata.newPosition;
        velocity = movedata.newVelocity;
        pmflags = movedata.flags;
        viewheight = movedata.viewheight;

        //update PlayerState
        PlayerState.currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        PlayerState.currentPosition = position;
        PlayerState.currentVelocity = velocity;
        //PlayerState.currentViewHeight = viewheight;

        if (movedata.jumped)
        {
            jumped = true;
        }

        transform.position = PlayerState.currentPosition;

        // Fetch debug variables
        currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        currentPosition = position;
        pm_gravity = PlayerState.pm_gravity;
        //jumpHeld = pmflags.HasFlag(PMFlags.PMF_JUMP_HELD);
        onGround = pmflags.HasFlag(PMFlags.PMF_ON_GROUND);


    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound[UnityEngine.Random.Range(0, shootSound.Length)]);
    }

    public IEnumerator DamageLight()
    {
        light.enabled = true;
        yield return new WaitForSeconds(0.2f); // wait for the next frame
        light.enabled = false;
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
        PlayerState.currentPosition = position;
        //PlayerState.currentViewHeight = viewheight;

        if (jumped)
        {
            audioSource.PlayOneShot(jumpSound);
            jumped = false;
        }

        // Fetch debug variables
    }
}