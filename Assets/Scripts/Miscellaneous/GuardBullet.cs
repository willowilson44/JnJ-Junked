using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBullet : MonoBehaviour
{
    private int collisionCount = 0;
    private int maxCollisions = 2;
    private float knockbackForce = 5f;
    private int damageAmount = 5; // Change this value to whatever amount of damage you want the bullet to deal
    private bool destroying;
    private Rigidbody rb;
    public AudioClip ricochetSound;
    public AudioClip collisionSound;
    private AudioSource audioSource;
    private Light bulletLight;
    private Vector3 knockbackDirection; // Use bullet's velocity for knockback direction

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If not, add an AudioSource component to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        bulletLight = GetComponent<Light>();
        knockbackDirection = rb.velocity.normalized;
    }

    private void Update()
    {
        if (rb.velocity != Vector3.zero) // Ensure that the velocity is not zero to avoid LookRotation with a zero vector.
        {
            Quaternion rotation = Quaternion.LookRotation(rb.velocity.normalized);
            transform.rotation = Quaternion.Euler(90, rotation.eulerAngles.y, rotation.eulerAngles.z); // adjust the X rotation
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (destroying)
        {
            return;
        }

        collisionCount++;

        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damageAmount);
            }

            // Apply a knockback force to the enemy
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }

            bulletLight.intensity *= 5;
            audioSource.PlayOneShot(collisionSound);
            StartCoroutine(DelayedDestroy());  // Destroy bullet upon hitting an enemy
            return;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerState.Damage(damageAmount);

            bulletLight.intensity *= 5;
            audioSource.PlayOneShot(collisionSound);
            StartCoroutine(DelayedDestroy());  // Destroy bullet upon hitting an enemy
            return;
        }
        else
        {
            audioSource.PlayOneShot(ricochetSound);
        }

        if (collisionCount >= maxCollisions)
        {
            bulletLight.intensity *= 5;
            StartCoroutine(DelayedDestroy());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger entered)");
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("is enemy");
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {

                //Debug.Log("startling enemy");
                enemyHealth.StartCoroutine(enemyHealth.Startle());
            }
        }
    }

    private IEnumerator DelayedDestroy()
    {
        destroying = true;

        // Freeze the Rigidbody's motion
        if (rb != null) rb.isKinematic = true;

        // Disable the MeshRenderer
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = false;

        yield return new WaitForSeconds(0.15f);  // Wait for the duration of the collision sound
        Destroy(this.gameObject);
    }
}