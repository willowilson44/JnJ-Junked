using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
 * Author: Josh Wilson
 * 
 * Instructions:
 *  - None
 * 
 * Description:
 *  - This script is a collection of shooting functions used by the PlayerActionUpdate script. 
 * 
 */

public static class PlayerShooting
{
    private static Vector3 target;
    private static Vector3 gunPoint;
    public static float bulletSpeed = 80f; // Speed of the bullet
    private static float lastFiredTime = 0f; // Time the player last fired
    private static float fireRate = 0.7f; // Fire rate in seconds


    public static void DoShoot(GameObject playerObject, Camera camera)
    {
        // Check if enough time has passed since the last shot
        if (Time.time - lastFiredTime < fireRate)
        {
            return;
        }

        // Record the current time
        lastFiredTime = Time.time;


        Transform gunTransform = playerObject.transform.Find("Model/BasicCannonArmUnTexed");
        gunPoint = gunTransform.position + gunTransform.right * -1f; // + gunTransform.forward * -0.8f

        GetTargetLocation(camera);
        // Instantiate "Bullet1" prefab at gunpoint location and shoot it at Target Location
        // Assumes a Bullet1 prefab with a Rigidbody component
        GameObject bulletPrefab = Resources.Load<GameObject>("Bullet1");
        GameObject bulletInstance = Object.Instantiate(bulletPrefab, gunPoint, gunTransform.rotation * Quaternion.Euler(0, 0, 90));
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>(); 
        Bullet1 bulletScript = bulletInstance.GetComponent<Bullet1>();
        bulletScript.player = playerObject;

        Vector3 direction = (target - gunPoint).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    public static void GetTargetLocation(Camera camera)
    {
        // Assuming the middle of the screen is the aim point
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        // Assume everything but the player is on the default layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
        {
            target = hit.point;
        }
        else
        {
            // Set the target to a point in the distance along the camera's forward vector
            target = ray.origin + ray.direction * 1000f;
        }
    }
}
