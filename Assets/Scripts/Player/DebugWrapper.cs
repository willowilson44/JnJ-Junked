using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Instructions:
 *  - Attach to player character
 *  
 * Description:
 *  - Exposes several player state variables to inspector for debugging
 *  
 */

public class DebugWrapper : MonoBehaviour
{
    // Variables to expose in the Unity Editor
    public float currentSpeed;
    //public float currentViewHeight;
    public Vector3 currentPosition;
    public float pm_gravity;
    //public Vector3 addVelocities;
    //public int onGround;
    //public int jumpHeld;

    //Health related
    public int startingEnergy;
    public int batteryEnergy;
    public int currentMax;
    public int currentEnergy;
    public bool isAlive;

    public float currentSpeedModifier;

    //Upgrade related
    public bool canJump;
    public bool canShoot;
    public bool canDoubleJump;

    // Update is called once per frame
    private void Update()
    {
        // Fetch the variables from your PlayerState static class
        currentSpeed = PlayerState.currentSpeed;
        currentPosition = PlayerState.currentPosition;
        pm_gravity = PlayerState.pm_gravity;
        startingEnergy = PlayerState.startingEnergy;
        batteryEnergy = PlayerState.batteryEnergy;
        currentMax = PlayerState.currentMax;
        currentEnergy = PlayerState.currentEnergy;
        isAlive = PlayerState.isAlive;
        currentSpeedModifier = PlayerState.currentSpeedModifier;
        canJump = PlayerState.canJump;
        canShoot = PlayerState.canShoot;
        canDoubleJump = PlayerState.canDoubleJump;
}
}
