using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Author: Josh Wilson
 * 
 * Instructions:
 *  - Attach this script to main camera
 *  - Create a game object as a child of the player and position it where you want the camera to be
 *  - Assign that object as "Target" in inspector
 *  
 *  Description:
 *  - Player Camera movement
 *  
 */

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f;
    public float lookSpeedX = 8f;
    public float lookSpeedY = 8f;
    public float gamepadSensitivityX = 80f;
    public float gamepadSensitivityY = 30f;
    private string controlDevice;
    private Vector2 currentLookDelta;
    private InputMaster controls;
    private float pitch = 0f;
    public float yRotation;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Look.performed += ctx => {
            currentLookDelta = ctx.ReadValue<Vector2>();
            controlDevice = ctx.control.device.name;
        };
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

    public float DoLook()
    {
        // Follow the player
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeed);

        // Apply sensitivity multiplier if using gamepad
        //float sensitivityMultiplier = controlDevice.Contains("Mouse") ? 1 : gamepadSensitivity;

        // Camera rotation
        float xRotation;

        if (controlDevice.Contains("Mouse")) 
        {
            yRotation = currentLookDelta.x * lookSpeedX * Time.deltaTime;
            xRotation = currentLookDelta.y * lookSpeedY * Time.deltaTime;
        } else
        {
            yRotation = currentLookDelta.x * gamepadSensitivityX * Time.deltaTime;
            xRotation = currentLookDelta.y * gamepadSensitivityY * Time.deltaTime;
        }

        pitch -= xRotation;
        pitch = Mathf.Clamp(pitch, -40f, 85f);

        transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y + yRotation, 0);
        return yRotation;
    }
}
