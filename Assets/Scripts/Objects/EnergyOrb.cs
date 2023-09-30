using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyOrb : MonoBehaviour
{
    private GameObject player;
    public int detectionRange = 15;         // Within this range the orb will chase the player
    public int healthAmount = 4;         // Within this range the orb will chase the player
    public float moveSpeed = 2.0f;
    public AudioClip clip;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        player = ReferenceManager.instance.player;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If not, add an AudioSource component to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            // Move the orb towards the player
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && (PlayerState.currentMax != PlayerState.currentEnergy))
        {
            PlayerState.AddPower(healthAmount);
            audioSource.PlayOneShot(clip);
            Destroy(this.gameObject);
        }

        if (other.tag == "Enemy")
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.AddPower(healthAmount*2);
            Destroy(this.gameObject);
        }
    }


}
