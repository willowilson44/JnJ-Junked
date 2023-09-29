using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script stores progress/state information for the current level playthrough.
 *  
 */

public static class LevelState
{
    // current level state
    public static int currentDifficulty = 0;
    public static int currentLevel = 0;
    public static int kills = 0;
    public static int deaths = 0;
    public static int newUpgrades = 0;
    public static bool devMode = false;


    public static void ResetGameState()
    {
        kills = 0;
        deaths = 0;
        newUpgrades = 0;
    }

    public static void PlayerDeath()
    {
        deaths++;
        Debug.Log("The player died... " + deaths + " deaths");

        // fade to black sequence

        // respawn the player at the level start
    }

    public static void EnemyKilled()
    {
        kills++;
        Debug.Log(kills + " kills");
    }

    public static void UpgradeCollected()
    {
        newUpgrades++;
        Debug.Log(newUpgrades + " upgrades found on this level");
    }
}
