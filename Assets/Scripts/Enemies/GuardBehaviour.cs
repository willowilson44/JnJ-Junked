using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;
    private AudioSource audioSource;

    // Enemy Movement
    public float speed = 4.0f;          // Enemy speed
    public float turningSpeed = 80f;   // Enemy turning speed
    public float acceleration = 5f;    // Enemy acceleration
    private float predictionTime = 1f;   // How far ahead of the player the enemy tries to predict movement

    // Detecting the Player
    //private bool isListening = false;        // if true enemy will raycast to try to "see" the player
    public bool isChasing = false;          // if true enemy will chase/attack the player
    //private bool isAttacking = false;        // if true enemy will chase/attack the player
    private bool canAlert = true;
    public int detectionRange = 60;         // Within this range the enemy will raycast to try to "see" the player
    private int detectBehindRange = 10;      // Within this range the enemy will immediately find the player even if behind it
    public float sightCone = 90f;           // Degrees width of sight cone from forward (90 = directly to the side)
    private int chaseDuration = 5;           // How long (seconds) enemy targets the player before needing to check if it can still see the player
    private float timeLastSighted;
    EnemyHealth enemyHealthComponent;
    public AudioClip[] alertSounds = new AudioClip[6];

    // Shooting at the player
    private static Vector3 target;
    private static Vector3 futureTarget;
    private static Vector3 gunPoint;
    private float bulletSpeed = 22f; // Speed of the bullet
    private float lastFiredTime = 0f; // Time the player last fired
    private float fireRate = 0.3f; // Fire rate in seconds
    //private int shootRange = 50;             // Within this range the enemy will initiate an attack
    private float shootCone = 40f;           // Degrees width of attack cone from forward (90 = directly to the side)
    private float shootPredictionTime = 1.5f;   // How far ahead of the player the enemy tries to predict movement
    public AudioClip shootSound;


    // Start is called before the first frame update
    void Start()
    {
        player = ReferenceManager.instance.player;
        enemyHealthComponent = GetComponent<EnemyHealth>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If not, add an AudioSource component to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.6f;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed + (LevelState.currentDifficulty);       //scales AI speed to difficulty
        agent.angularSpeed = turningSpeed;                              //scale these too??
        agent.acceleration = acceleration;                              //scale these too??

        bulletSpeed = bulletSpeed - (2 * (2 - LevelState.currentDifficulty));   //Scale bullet speed by difficulty
        fireRate = fireRate + (0.1f* (2 - LevelState.currentDifficulty));
    }

    // Update is called once per frame
    void Update()
    {
        // Check if enemy is startled via detection of bullet in it's enemy health script
        if (enemyHealthComponent.startled)
        {
            isChasing = true;
            timeLastSighted = Time.time;   // Record the time when the player was last sighted
        }

        // Check if player can be found
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

        // Check if enough time has passed since the last shot
        if (Time.time - lastFiredTime > fireRate)      // previously (Time.time - lastFiredTime > fireRate && distanceToPlayer < shootRange)
        {
            TryShoot();
        }

        if (canAlert)
        {
            StartCoroutine(AlertNoise());
        }
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

    public void TryShoot()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;    // Calculate the direction to the player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle <= shootCone)
        {
            // Record the current time
            lastFiredTime = Time.time;

            Transform gunTransform = gameObject.transform.Find("GuardUntexed/Armature/Body/Body_end");
            gunPoint = gunTransform.position + gunTransform.forward * -1f; // + gunTransform.forward * -0.8f

            // Instantiate bullet prefab at gunpoint location and shoot it at Target Location
            // Assumes a prefab with a Rigidbody component
            GameObject bulletPrefab = Resources.Load<GameObject>("GuardBullet");
            GameObject bulletInstance = Object.Instantiate(bulletPrefab, gunPoint, Quaternion.LookRotation(directionToPlayer) * Quaternion.Euler(90, 0, 0));
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

            //Could be used for tallying kills
            //Bullet1 bulletScript = bulletInstance.GetComponent<Bullet1>();
            //bulletScript.player = playerObject;     

            target = player.transform.position;
            Vector3 playerVelocity = PlayerState.currentVelocity;
            futureTarget = PlayerState.currentPosition + (playerVelocity / 40) * (shootPredictionTime * (0.1f * UnityEngine.Random.Range(3, 11)));

            // Decide randomly whether to aim at current position or future position
            int choice = UnityEngine.Random.Range(0, 3); // Generates 0,1 or 2

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
        int waitInt = UnityEngine.Random.Range(10, 20);
        yield return new WaitForSeconds(waitInt);

        canAlert = true;
    }
}