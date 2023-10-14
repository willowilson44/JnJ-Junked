using System.Linq;
using System.Text;
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
    public int difficulty;
    public int levelNumber;

    //Health related
    public int startingEnergy;
    public int batteryEnergy;
    public int currentMax;
    public int currentEnergy;
    public bool isAlive;

    public float currentSpeedModifier;
    public float upgradeSpeedModifier;

    //Upgrade related
    public bool canJump;
    public bool canShoot;
    public bool canDoubleJump;

    [TextArea]
    public string levelsCompletedStr;
    public string batStr;
    public string upStr;


    // Update is called once per frame
    private void Update()
    {
        // Fetch the variables from your PlayerState static class
        currentSpeed = PlayerState.currentSpeed;
        currentSpeedModifier = PlayerState.currentSpeedModifier;
        upgradeSpeedModifier = PlayerState.upgradeSpeedModifier;
        currentPosition = PlayerState.currentPosition;
        pm_gravity = PlayerState.pm_gravity;
        currentMax = PlayerState.currentMax;
        currentEnergy = PlayerState.currentEnergy;
        isAlive = PlayerState.isAlive;
        canJump = PlayerState.canJump;
        canShoot = PlayerState.canShoot;
        canDoubleJump = PlayerState.canDoubleJump;
        levelsCompletedStr = ArrayToString(GameSettings.levelsCompleted);
        batStr = ArrayToString(GameSettings.batteriesFound);
        upStr = ArrayToString(GameSettings.upgradesFound);
        levelNumber = LevelState.currentLevel;
        difficulty = LevelState.currentDifficulty;
    }

    private string ArrayToString(bool[][] array)
    {
        var str = new StringBuilder();
        foreach (var row in array)
        {
            str.AppendLine(string.Join(", ", row.Select(b => b.ToString()).ToArray()));
        }
        return str.ToString();
    }
}
