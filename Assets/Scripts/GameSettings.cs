using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script stores player progress for each level and difficulty, and upgrades collected in each difficulty level.
 *  - Progress is saved so that the player can resume the next time they play without losing upgrades/level progress.
 *  
 *  - Instructions for adding upgrades:
 *  -       Update numUpgrades, the upgrade list below, and then go to the UpgradeSelector script and add the 
 *  -       upgrade in the UpdateSlotLists() and TogglePiece() functions
 */

/* 
 * current upgrade list:
 * upgradesFound[difficulty][0] = Jump Upgrade (legs)
 * upgradesFound[difficulty][1] = Gun Upgrade (right arm)
 * upgradesFound[difficulty][2] = Hyper Blaster Upgrade (right Arm)
 * upgradesFound[difficulty][3] = Heavy Armor Upgrade (body)
 * upgradesFound[difficulty][4] = Gravitron Armor Upgrade (body)
 * upgradesFound[difficulty][5] = Power Armor Upgrade (body)
 * upgradesFound[difficulty][6] = Torch (left Arm)
 * upgradesFound[difficulty][7] = Double Jump Upgrade (legs)
*/

public static class GameSettings
{
    // game constants
    public const int numLevels = 3;
    public static readonly string[] levelNames = { "Level1", "Level2", "Level3" };
    public static readonly string[] upGradeNames = { "Jump", "Blaster", "Hyper Blaster", "Heavy Armor", "Gravitron Armor", "Power Armor", "Torch", "Double Jump" };
    public const int numDifficulties = 3;
    private const int numUpgrades = 8;
    private const int numBatteries = 4;
    public const int batteryEnergy = 10;    // The energy gained from each battery
    public const int startingEnergy = 70;


    // saved settings
    public static bool[][] levelsCompleted;     // indexed by: [level] [difficulty]
    public static bool[][] batteriesFound;       // indexed by: [difficulty] [numBatteries]
    public static bool[][] upgradesFound;       // indexed by: [difficulty] [numUpgrades]




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

        LoadGameState();
    }

    public static int getBatteryPower()
    {
        int count = 0;
        foreach (bool battery in batteriesFound[LevelState.currentDifficulty])
        {
            if (battery == true)
            {
                count++;
            }
        }
        return startingEnergy + (count * batteryEnergy);
    }

    public static void BatteryCollected(int batteryNumber)
    {
        if (batteriesFound[LevelState.currentDifficulty][batteryNumber] == false)
        {
            batteriesFound[LevelState.currentDifficulty][batteryNumber] = true;
            PlayerState.UpdateEnergyMax();
            LevelState.UpgradeCollected();
        }
        PlayerState.AddPower(10);
    }

    public static void UpgradeCollected(int upgradeNumber)
    {
        if (upgradesFound[LevelState.currentDifficulty][upgradeNumber] == false)
        {
            upgradesFound[LevelState.currentDifficulty][upgradeNumber] = true;
            LevelState.UpgradeCollected();
        }
    }

    // Save game state to PlayerPrefs
    public static void SaveGameState()
    {
        for (int i = 0; i < numLevels; i++)
        {
            for (int j = 0; j < numDifficulties; j++)
            {
                string levelKey = $"level_{i}_{j}";
                PlayerPrefs.SetInt(levelKey, levelsCompleted[i][j] ? 1 : 0);
            }
        }

        for (int i = 0; i < numDifficulties; i++)
        {
            for (int j = 0; j < numBatteries; j++)
            {
                string batteryKey = $"battery_{i}_{j}";
                PlayerPrefs.SetInt(batteryKey, batteriesFound[i][j] ? 1 : 0);
            }

            for (int k = 0; k < numUpgrades; k++)
            {
                string upgradeKey = $"upgrade_{i}_{k}";
                PlayerPrefs.SetInt(upgradeKey, upgradesFound[i][k] ? 1 : 0);
            }
        }


        PlayerPrefs.Save();
    }

    // Load game state from PlayerPrefs
    public static void LoadGameState()
    {
        for (int i = 0; i < numLevels; i++)
        {
            for (int j = 0; j < numDifficulties; j++)
            {
                string levelKey = $"level_{i}_{j}";
                levelsCompleted[i][j] = PlayerPrefs.GetInt(levelKey, 0) == 1;
            }
        }

        for (int i = 0; i < numDifficulties; i++)
        {
            for (int j = 0; j < numBatteries; j++)
            {
                string batteryKey = $"battery_{i}_{j}";
                batteriesFound[i][j] = PlayerPrefs.GetInt(batteryKey, 0) == 1;
            }

            for (int k = 0; k < numUpgrades; k++)
            {
                string upgradeKey = $"upgrade_{i}_{k}";
                upgradesFound[i][k] = PlayerPrefs.GetInt(upgradeKey, 0) == 1;
            }
        }

    }
}