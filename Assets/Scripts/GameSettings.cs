using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script stores player progress for each level and difficulty, and upgrades collected in each difficulty level.
 *  - Progress is saved so that the player can resume the next time they play without losing upgrades/level progress.
 *  
 */

public static class GameSettings
{
    // game constants
    private const int numLevels = 3;
    private const int numDifficulties = 3;
    private const int numUpgrades = 3;
    private const int numBatteries = 3;

    // saved settings
    public static bool[][] levelsCompleted;     // indexed by: [level] [difficulty]
    public static bool[][] batteriesFound;       // indexed by: [difficulty] [numBatteries]
    public static bool[][] upgradesFound;       // indexed by: [difficulty] [numUpgrades]

    /* 
     * upgrade list:
     * upgradesFound[difficulty][0] = Jump Upgrade
     * upgradesFound[difficulty][1] = Gun Upgrade
     * upgradesFound[difficulty][2] = Double Jump Upgrade
    */


    // settings loader/constructor
    static GameSettings()
    {
        levelsCompleted = new bool[numLevels][];
        for (int i = 0; i < numLevels; i++)
        {
            levelsCompleted[i] = new bool[numDifficulties];
        }

        upgradesFound = new bool[numDifficulties][];
        for (int i = 0; i < numDifficulties; i++)
        {
            upgradesFound[i] = new bool[numUpgrades];
        }

        batteriesFound = new bool[numDifficulties][];
        for (int i = 0; i < numDifficulties; i++)
        {
            batteriesFound[i] = new bool[numBatteries];
        }
    }

    public static int getBatteryPower()
    {
        int count = 0;
        foreach (bool battery in batteriesFound[LevelState.currentDifficulty]) {
            if (battery == true)
            {
                count++;
            }
        }
        return count;
    }

    public static void BatteryCollected(int batteryNumber)
    {
        if (batteriesFound[LevelState.currentDifficulty][batteryNumber] == false)
        {
            batteriesFound[LevelState.currentDifficulty][batteryNumber] = true;
            PlayerState.UpdatePower();
            LevelState.UpgradeCollected();
        }
        PlayerState.AddPower(10);
    }

    public static void UpgradeCollected(int upgradeNumber)
    {
        if (upgradesFound[LevelState.currentDifficulty][upgradeNumber] == false)
        {
            batteriesFound[LevelState.currentDifficulty][upgradeNumber] = true;
            PlayerState.UpdateUpgrades();
            LevelState.UpgradeCollected();
        }
    }
}
