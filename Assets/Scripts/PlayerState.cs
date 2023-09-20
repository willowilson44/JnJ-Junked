using System;
using UnityEngine;
using UnityEngine.Events;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script stores the current state information for the player during a game.
 *  
 */

public static class PlayerState
{

    //Player Movement State
    public static float pm_gravity = 1000;
    public static Vector3 currentPosition = Vector3.zero;
    public static Vector3 currentVelocity = Vector3.zero;
    public static PMFlags moveFlags;
    public static float currentSpeed = 0;
    //public static Vector3 addVelocities = Vector3.zero;
    //public static float currentViewHeight = 0;

    //Player Health
    public const int startingEnergy = 70;
    public const int batteryEnergy = 10;
    public static int currentMax = 70;
    public static int currentEnergy = 70;
    public static bool isAlive = true;

    //Player Speed modifier
    private const int defaultSpeedModifier = 30;
    public static int currentSpeedModifier;

    //Upgrade related
    public static bool canJump = false;
    public static bool canShoot = false;
    public static bool canDoubleJump = false;

    //Spawn
    //public static Vector3 spawnOrigin = Vector3.zero;
    //public static Vector3 spawnAngles = Vector3.right;

    //weapon pickup
    //public delegate void PickupWeapon(WeaponClassname weaponType);
    //public static PickupWeapon OnWeaponPickup;
    //public static UnityEvent teleportEvent = new UnityEvent();
    //public static Vector3 teleportDestination = Vector3.zero;
    //public static Vector3 teleportAngles = Vector3.zero;

    // Constructor - set initial state
    static PlayerState()
    {
        UpdateUpgrades();
        //UpdatePower();
        currentEnergy = currentMax;
        UpdateSpeed();
    }

    public static void UpdateUpgrades()
    {
        // Jump Upgrade
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][0] == false)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

        // Gun Upgrade
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][1] == false)
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
        // Double Jump Upgrade
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][2] == false)
        {
            canDoubleJump = false;
        }
        else
        {
            canDoubleJump = true;
        }
    }

    public static void UpdatePower()
    {
        currentMax = startingEnergy + (GameSettings.getBatteryPower() * batteryEnergy);
    }

    public static void UpdateSpeed()
    {
        currentSpeedModifier = defaultSpeedModifier + (20 - (currentEnergy / 5));
        PlayerMovement.pm_scaling_factor = currentSpeedModifier;
    }

    public static void AddPower(int collected)
    {
        int newEnergy = currentEnergy + collected;

        if(newEnergy < currentMax)
        {
            currentEnergy = newEnergy;
        } else
        {
            currentEnergy = currentMax;
        }
    }
    public static void DeductPower(int damage)
    {
        int newEnergy = currentEnergy - damage;

        if (newEnergy > 0)
        {
            currentEnergy = newEnergy;
            UpdateSpeed();
        }
        else
        {
            currentEnergy = 0;
            isAlive = false;
            LevelState.PlayerDeath();
        }
    }
    public static void ResetState()
    {
        pm_gravity = 1000;
        currentPosition = Vector3.zero;
        currentVelocity = Vector3.zero;
        currentSpeed = 0;
        isAlive = true;
        UpdateUpgrades();
        UpdatePower();
        currentEnergy = currentMax;
        PlayerMovement.pm_scaling_factor = defaultSpeedModifier;
    }
}
