using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script is attached to upgrades sitting in the game world awaiting collection. When collected the upgrade will be added to the player and game settings.
 *  
 */

/* 
 * current battery list:        (Batteries still need to be tracked by number to ensure players don't double dip by replaying levels)
 * batteriesFound[difficulty][0] = Level 1
 * batteriesFound[difficulty][1] = Level 2 a
 * batteriesFound[difficulty][2] = Level 2 b
 * batteriesFound[difficulty][3] = Level 3
*/

public class Battery : MonoBehaviour
{
    public float rotationSpeed = 20f; // The speed at which the object rotates.
    public AudioSource audioSource;
    public AudioClip pickupSound;

    public int batteryNumber;

    void Start()
    {
        if (GameSettings.batteriesFound[LevelState.currentDifficulty][batteryNumber] == true)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Rotate the upgrade around its Y-axis.
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        // Check if the object that entered the trigger is the player.
        if (other.CompareTag("Player"))
        {
            Collected(other.gameObject);
        }
    }

    private void Collected(GameObject player)
    {
        // Play the pickup sound.
        if (pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        Notifications notificationSystem = FindObjectOfType<Notifications>();
        notificationSystem.DisplayNotification("You've found a Battery Upgrade!\n\nYour maximum increased by 10 and you are feeling faster!!", 4.0f); // This message will last for 3 seconds


        // Perform the upgrade and other related tasks.
        GameSettings.BatteryCollected(batteryNumber);
        PlayerState.UpdateEnergyMax();

        // After performing the upgrade tasks, deactivate or destroy the upgrade GameObject.
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }
}
