using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script defines the Dalek AI behaviour towards the player, including awareness of the player, chasing, and shooting at the player by instantiating bullets.
 *  
 */

public class DalekBehaviour : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

    // Enemy Attack
    public int damage = 20;
    private float attackRate = 2f;
    private float attackCone = 60f;           // Degrees width of attack cone from forward (90 = directly to the side)
    private int attackRange = 2;             // Within this range the enemy will initiate an attack
    private float lastAttackTime = 0f;
    private Rigidbody rb;
    public AudioClip shootSound;
    public AudioClip[] alertSounds = new AudioClip[6];
    private AudioSource audioSource;

    // Enemy Movement
    public float speed = 4.0f;          // Enemy speed
    public float turningSpeed = 120f;   // Enemy turning speed
    public float acceleration = 10f;    // Enemy acceleration
    private float predictionTime = 0.4f;   // How far ahead of the player the enemy tries to predict movement

    // Detecting the Player
    private bool isListening = false;        // if true enemy will raycast to try to "see" the player
    private bool isChasing = false;          // if true enemy will chase/attack the player
    private bool isAttacking = false;        // if true enemy will chase/attack the player
    private bool canAlert = true;
    public int detectionRange = 35;         // Within this range the enemy will raycast to try to "see" the player
    private int detectBehindRange = 10;      // Within this range the enemy will immediately find the player even if behind it
    public float sightCone = 90f;           // Degrees width of sight cone from forward (90 = directly to the side)
    private int chaseDuration = 5;           // How long (seconds) enemy targets the player before needing to check if it can still see the player
    private float timeLastSighted;
    EnemyHealth enemyHealthComponent;

    // Shooting at the player
    private static Vector3 target;
    private static Vector3 futureTarget;
    private static Vector3 gunPoint;
    private float bulletSpeed = 26f; // Speed of the bullet
    private float lastFiredTime = 0f; // Time the player last fired
    private float fireRate = 2f; // Fire rate in seconds
    private int shootRange = 35;             // Within this range the enemy will initiate an attack
    private float shootCone = 25f;           // Degrees width of attack cone from forward (90 = directly to the side)
    private float shootPredictionTime = 0.6f;   // How far ahead of the player the enemy tries to predict movement


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

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed + (LevelState.currentDifficulty * 2);       //scales AI speed to difficulty
        agent.angularSpeed = turningSpeed;                              //scale these too??
        agent.acceleration = acceleration;                              //scale these too??
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
                isListening = true;

                // Check if the player is VERY close behind
                if (distanceToPlayer <= detectBehindRange)
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
                isListening = false;
            }
        }
    }

    void PursuePlayer()
    {
        Vector3 playerVelocity = PlayerState.currentVelocity;
        Vector3 futurePosition = PlayerState.currentPosition + (playerVelocity / 40) * predictionTime;
        agent.SetDestination(futurePosition);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange && (Time.time - lastAttackTime > attackRate))           //Can enemy can attack now?
        {
            StartCoroutine(AttackPlayer());
        }

        // Check if enough time has passed since the last shot
        if (Time.time - lastFiredTime > fireRate && distanceToPlayer < shootRange)
        {
            TryShoot();
        }

        if (canAlert)
        {
            StartCoroutine(AlertNoise());
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
            audioSource.PlayOneShot(alertSounds[UnityEngine.Random.Range(0, alertSounds.Length)]);
            PlayerState.Damage(damage + (5 * LevelState.currentDifficulty));

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
                if (hit.collider.gameObject == player)
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

    public void TryShoot()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;    // Calculate the direction to the player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle <= shootCone)
        {
            // Record the current time
            lastFiredTime = Time.time;

            Transform gunTransform = gameObject.transform.Find("Base/Middle/RightProdderRotationJoint");
            gunPoint = gunTransform.position + gunTransform.forward * -2f; // + gunTransform.forward * -0.8f

            // Instantiate bullet prefab at gunpoint location and shoot it at Target Location
            // Assumes a prefab with a Rigidbody component
            GameObject bulletPrefab = Resources.Load<GameObject>("DalekBullet");
            GameObject bulletInstance = Object.Instantiate(bulletPrefab, gunPoint, Quaternion.LookRotation(directionToPlayer) * Quaternion.Euler(90, 0, 0));
            //GameObject bulletInstance = Object.Instantiate(bulletPrefab, gunPoint, gunTransform.rotation * Quaternion.Euler(90, 0, 0));
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            Bullet1 bulletScript = bulletInstance.GetComponent<Bullet1>();
            //bulletScript.player = playerObject;     //could be used for tallying kills

            target = player.transform.position;
            Vector3 playerVelocity = PlayerState.currentVelocity;
            futureTarget = PlayerState.currentPosition + (playerVelocity / 40) * (shootPredictionTime * (0.1f * UnityEngine.Random.Range(3, 11)));

            // Decide randomly whether to aim at current position or future position
            int choice = UnityEngine.Random.Range(0, 3); // Generates 0 or 1

            Vector3 finalTarget;
            if (choice == 0)
            {
                finalTarget = target;
            }
            else
            {
                finalTarget = futureTarget;
            }

            Vector3 direction = (finalTarget - gunPoint).normalized;
            rb.velocity = direction * bulletSpeed;


            audioSource.PlayOneShot(shootSound);
        }
    }

    private IEnumerator AlertNoise()
    {
        audioSource.PlayOneShot(alertSounds[UnityEngine.Random.Range(0, alertSounds.Length)]);
        canAlert = false;
        int waitInt = UnityEngine.Random.Range(7, 13);
        yield return new WaitForSeconds(waitInt);

        canAlert = true;
    }
}