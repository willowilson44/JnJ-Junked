using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script is attached to upgrades sitting in the game world awaiting collection. When collected the upgrade will be added to the player and game settings.
 *  
 */

/* 
 * current upgrade list:
 * upgradesFound[difficulty][0] = Jump Upgrade
 * upgradesFound[difficulty][1] = Gun Upgrade
 * upgradesFound[difficulty][2] = Double Jump Upgrade
*/

public class Upgrade : MonoBehaviour
{
    public float rotationSpeed = 20f; // The speed at which the object rotates.
    public AudioClip pickupSound; // The sound to play when the upgrade is picked up.
    public int upgradeNumber;
    public AudioSource audioSource;

    void Start()
    {
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][upgradeNumber] == true)
        {
            gameObject.SetActive(false);
        } 
    }

    void Update()
    {
        // Rotate the upgrade around its Y-axis.
        transform.parent.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
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
        notificationSystem.DisplayNotification("You've found the " + GameSettings.upGradeNames[upgradeNumber] + " Upgrade!! Equipping now...", 4.0f); // This message will last for 3 seconds

        // Perform the upgrade and other related tasks.
        GameSettings.UpgradeCollected(upgradeNumber);

        UpgradeSelector playerUpSelector = player.GetComponent<UpgradeSelector>();
        playerUpSelector.UpdateSlotLists();

        // After performing the upgrade tasks, deactivate or destroy the upgrade GameObject.
        gameObject.SetActive(false);
        // Destroy(gameObject);

        // Also Set the upgrade instantly to active
    }
}