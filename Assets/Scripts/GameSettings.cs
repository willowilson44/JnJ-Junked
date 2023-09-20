using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    // game constants
    private const int numLevels = 3;
    private const int numDifficulties = 3;
    private const int numUpgrades = 6;

    // saved settings
    public static bool[][] levelsCompleted;     // indexed by: [level] [difficulty]
    public static bool[][] upgradesFound;       // indexed by: [difficulty] [upgradeNumber]

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
            levelsCompleted[i] = new bool[numUpgrades];
        }
    }
}
