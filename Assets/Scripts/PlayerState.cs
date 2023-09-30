using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public static int currentSpeedModifier;
    //public static Vector3 addVelocities = Vector3.zero;
    //public static float currentViewHeight = 0;

    //Player Health
    public static int currentMax = 70;
    public static int currentEnergy = 70;
    public static bool isAlive = true;
    private static TMP_Text energyText; // Reference to the TextMeshPro component
    private static Image maxEnergyImage;
    private static Image currentEnergyImage;


    //Player Speed Modifiers
    private const int defaultSpeedModifier = 30;
    public static float damageSpeedModifier = 30;      // Maximum amount to slow the player by when damaged

    //Upgrade Effects
    public static bool canJump = false;
    public static bool canShoot = false;
    public static bool canDoubleJump = false;
    public static int selectedLegSpeedModifier = 0;     // +5 = noticeably slower, -5 = noticeably faster

    //Spawn
    //public static Vector3 spawnOrigin = Vector3.zero;
    //public static Vector3 spawnAngles = Vector3.right;
    //public static UnityEvent teleportEvent = new UnityEvent();
    //public static Vector3 teleportDestination = Vector3.zero;
    //public static Vector3 teleportAngles = Vector3.zero;

    // Constructor - set initial state
    static PlayerState()
    {
        //UpdatePower();
        currentEnergy = currentMax;
        UpdateSpeed();


        maxEnergyImage = ReferenceManager.instance.maxEnergyImage;
        maxEnergyImage.fillAmount = (float)currentMax / 120;

        currentEnergyImage = ReferenceManager.instance.currentEnergyImage;
        currentEnergyImage.fillAmount = (float)currentEnergy / 120;
    }


    public static void UpdateEnergyMax()
    {
        currentMax = GameSettings.getBatteryPower();

        maxEnergyImage = ReferenceManager.instance.maxEnergyImage;
        maxEnergyImage.fillAmount = (float)currentMax / 120;
    }

    public static void UpdateSpeed()
    {
        currentSpeedModifier = defaultSpeedModifier + selectedLegSpeedModifier + (int)(damageSpeedModifier - (currentEnergy / (100/damageSpeedModifier)));
        //Debug.Log("current: " + currentSpeedModifier);
        PlayerMovement.pm_scaling_factor = currentSpeedModifier;
    }

    public static void AddPower(int collected)
    {
        int newEnergy = currentEnergy + collected;

        if(newEnergy < currentMax)
        {
            UpdateEnergy(newEnergy);
        } else
        {
            UpdateEnergy(currentMax);
        }
    }

    public static void Damage(int damage)
    {
        int newEnergy = currentEnergy - damage;

        if (newEnergy > 0)
        {
            UpdateEnergy(newEnergy);
            UpdateSpeed();
        }
        else
        {
            UpdateEnergy(0);
            isAlive = false;
            LevelState.PlayerDeath();
            PlayerActionUpdate playerUpdate = ReferenceManager.instance.player.GetComponent<PlayerActionUpdate>();
            playerUpdate.Death();
            currentEnergy = currentMax;
        }
    }

    public static void UpdateEnergy(int newEnergy)
    {
        currentEnergy = newEnergy;
        energyText = GameObject.Find("GUI/Canvas/Energy Level").GetComponent<TMP_Text>();
        energyText.text = "Energy: "+ currentEnergy + " / " + currentMax;

        currentEnergyImage = ReferenceManager.instance.currentEnergyImage;
        currentEnergyImage.fillAmount = (float)currentEnergy / 120;

    }

    public static void ResetState()
    {
        pm_gravity = 1000;
        currentPosition = Vector3.zero;
        currentVelocity = Vector3.zero;
        currentSpeed = 0;
        isAlive = true;
        UpdateEnergyMax();
        currentEnergy = currentMax;
        PlayerMovement.pm_scaling_factor = defaultSpeedModifier;
    }
}
