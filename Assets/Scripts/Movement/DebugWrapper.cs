using UnityEngine;

public class DebugWrapper : MonoBehaviour
{
    // Variables to expose in the Unity Editor
    public float currentSpeed;
    public float currentViewHeight;
    public Vector3 currentPosition;
    public Vector3 mins;
    public Vector3 maxs;
    public float pm_gravity;
    public Vector3 addVelocities;
    public PMFlags moveFlags;

    // Update is called once per frame
    private void Update()
    {
        // Fetch the variables from your PlayerState static class
        currentSpeed = PlayerState.currentSpeed;
        currentViewHeight = PlayerState.currentViewHeight;
        currentPosition = PlayerState.currentPosition;
        mins = PlayerState.mins;
        maxs = PlayerState.maxs;
        pm_gravity = PlayerState.pm_gravity;
        addVelocities = PlayerState.addVelocities;
        moveFlags = PlayerState.moveFlags;
    }
}
