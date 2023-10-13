using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script defines the Scrapper AI behaviour towards the player, including awareness of the player, chasing, and attacking the player with a melee attack.
 *  
 */

public class ScrapperBehaviour : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

    // Enemy Attack
    public int damage = 5;          
    public float attackRate = 1.5f;
    public float attackCone = 60f;           // Degrees width of attack cone from forward (90 = directly to the side)
    private float lastAttackTime = 0f;
    //private float knockbackForce = 5f;
    private Rigidbody rb;
    //public AudioClip swingSound;
    public AudioClip[] hitSounds = new AudioClip[6];
    private AudioSource audioSource;

    // Enemy Movement
    public float speed = 6.0f;          // Enemy speed
    public float turningSpeed = 240f;   // Enemy turning speed
    public float acceleration = 15f;    // Enemy acceleration
    private float predictionTime = 0.7f;   // How far ahead of the player the enemy tries to predict movement

    // Detecting the Player
    //private bool isListening = false;        // if true enemy will raycast to try to "see" the player
    private bool isChasing = false;          // if true enemy will chase/attack the player
    //private bool isAttacking = false;        // if true enemy will chase/attack the player
    public int detectionRange = 45;         // Within this range the enemy will raycast to try to "see" the player
    private int detectBehindRange = 10;      // Within this range the enemy will immediately find the player even if behind it
    public float sightCone = 90f;           // Degrees width of sight cone from forward (90 = directly to the side)
    private int attackRange = 2;             // Within this range the enemy will initiate an attack
    private int chaseDuration = 5;           // How long (seconds) enemy targets the player before needing to check if it can still see the player
    private float timeLastSighted;
    EnemyHealth enemyHealthComponent;


    // Start is called before the first frame update
    void Start()
    {
        player = ReferenceManager.instance.player;
        enemyHealthComponent = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If not, add an AudioSource component to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.6f;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed + (LevelState.currentDifficulty * 2);       //scales AI speed to difficulty
        agent.angularSpeed = turningSpeed;                              //scale these too??
        agent.acceleration = acceleration;                              //scale these too??


        damage = damage * (LevelState.currentDifficulty + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealthComponent.startled)
        {
            isChasing = true;
            timeLastSighted = Time.time;   // Record the time when the player was last sighted
        }

        if (isChasing && Time.time - timeLastSighted < chaseDuration)
        {
            PursuePlayer();
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within detection range
            if (distanceToPlayer <= detectionRange)
            {
                //isListening = true;

                // Check if the player is VERY close behind
                if (distanceToPlayer <= detectBehindRange && LevelState.devMode == false)
                {
                    isChasing = true;
                    timeLastSighted = Time.time;   // Record the time when the player was last sighted
                    PursuePlayer();                // Perform immediate attack
                }
                else
                {
                    PerformRaycast();
                }
            }
            else
            {
                //isListening = false;
            }
        }
    }

    void PursuePlayer()
    {
        Vector3 playerVelocity = PlayerState.currentVelocity;
        Vector3 futurePosition = PlayerState.currentPosition + (playerVelocity / 40) * (predictionTime/2);
        agent.SetDestination(futurePosition);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange && (Time.time - lastAttackTime > attackRate))           //Can enemy can attack now?
        {
            StartCoroutine(AttackPlayer());
        }
    }
    private IEnumerator AttackPlayer()
    {
        //Debug.Log("Attacking player");
        lastAttackTime = Time.time;
        // Do Attack Animation here
        //audioSource.PlayOneShot(swingSound);

        agent.SetDestination(PlayerState.currentPosition);
        yield return new WaitForSeconds(0.2f);      // Lunging time during attack (keeps moving)

        agent.isStopped = true;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);   // Calculate the distance to the player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;    // Calculate the direction to the player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);                          // Calculate the angle between the AI's forward vector and the direction to the player

        if (distanceToPlayer <= attackRange && angle <= attackCone)
        {
            //Debug.Log("Hit Player");
            audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);

            if (PlayerState.powerArmor)
            {
                PlayerState.Damage(damage/2);
                enemyHealthComponent.Damage(damage);
            }
            else
            {
                PlayerState.Damage(damage);
            }

            PlayerActionUpdate playerActionUpdate = player.GetComponent<PlayerActionUpdate>();

            //// Apply a knockback force to the player
            //Vector3 knockbackDirection = rb.velocity.normalized;
            //if (playerActionUpdate != null)
            //{
            //    playerActionUpdate.ApplyKnockback(knockbackDirection, 10000);
            //    //enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            //}

            // Light Effect for player damage

            StartCoroutine(playerActionUpdate.DamageLight());
        }

        // Wait to end 
        yield return new WaitForSeconds(1f);      // Stationary time after attacking
        agent.isStopped = false;
    }

    void PerformRaycast()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Calculate the angle between the AI's forward vector and the direction to the player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle <= sightCone)  // Player within sight cone?
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
            {
                if (hit.collider.gameObject == player && LevelState.devMode == false)
                {
                    isChasing = true; 
                    timeLastSighted = Time.time;   // Record the time when the player was last sighted
                    PursuePlayer();                // Perform immediate attack
                }
                else
                {
                    isChasing = false;
                }
            }
        }
        else
        {
            isChasing = false;
        }
    }
}
