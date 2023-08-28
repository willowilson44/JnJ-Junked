using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 5.0f;
    private Rigidbody rb;
    private InputMaster controls;
    private Vector2 currentMovement;

    private void Awake()
    {
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
        // Strafe movement
        Vector3 movement = new Vector3(currentMovement.x, 0, currentMovement.y);
        Vector3 newPosition = rb.position + transform.TransformDirection(movement) * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Jumping
        if (controls.Player.Jump.triggered)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }
}
