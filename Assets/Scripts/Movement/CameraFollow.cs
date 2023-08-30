using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f;
    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;
    private Vector2 currentLookDelta;
    private InputMaster controls;
    private float pitch = 0f;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Look.performed += ctx => currentLookDelta = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => currentLookDelta = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void DoLook()
    {
        // Follow the player
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);

        // Camera rotation
        float yRotation = currentLookDelta.x * lookSpeedX * Time.deltaTime;
        float xRotation = currentLookDelta.y * lookSpeedY * Time.deltaTime;

        pitch -= xRotation;
        pitch = Mathf.Clamp(pitch, -40f, 85f);

        transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y + yRotation, 0);
    }
}
