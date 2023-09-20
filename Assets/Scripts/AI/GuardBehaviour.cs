using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class GuardBehaviour : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;
    //PlayerPositionUpdate playerPosition;

    public float speed = 8.0f; // Enemy speed
    public float turningSpeed = 240f; // Enemy turning speed
    public float acceleration = 15f; // Enemy turning speed
    public float predictionTime = 2f;

    public bool isListening = false;
    public bool isAttacking = false;
    public int detectionRange = 30;
    public int closeRange = 10;
    private float timeLastSighted;
    private int chaseDuration = 5;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //playerPosition = player.GetComponent<PlayerPositionUpdate>();

        agent.speed = speed;
        agent.angularSpeed = turningSpeed;
        agent.acceleration = acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking && Time.time - timeLastSighted < chaseDuration)
        {
            pursuePlayer();
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within detection range
            if (distanceToPlayer <= detectionRange)
            {
                isListening = true;

                // Check if the player is VERY close behind
                if (distanceToPlayer <= closeRange)
                {
                    isAttacking = true;
                    timeLastSighted = Time.time;   // Record the time when the player was last sighted
                    pursuePlayer();                // Perform immediate attack
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



        // Debugging logs
        // Debug.Log("Player Velocity: " + playerVelocity);
        // Debug.Log("Player Position: " + playerPosition.position);
        // Debug.Log("Predicted Future Position: " + futurePosition);


    }

    void pursuePlayer()
    {
        Vector3 playerVelocity = PlayerState.currentVelocity;
        Vector3 futurePosition = PlayerState.currentPosition + (playerVelocity / 40) * predictionTime;
        agent.SetDestination(futurePosition);
    }


    void PerformRaycast()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Calculate the angle between the AI's forward vector and the direction to the player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle <= 90f)  // Degrees for "cone of sight"
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
            {
                if (hit.collider.gameObject == player)
                {
                    isAttacking = true; 
                    timeLastSighted = Time.time;   // Record the time when the player was last sighted
                    pursuePlayer();                // Perform immediate attack
                }
                else
                {
                    isAttacking = false;
                }
            }
        }
        else
        {
            isAttacking = false;
        }
    }
}
