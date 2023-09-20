using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - This script defines the Guard AI behaviour towards the player.
 *  
 */

public class GuardBehaviour : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

    // Enemy movement
    public float speed = 6.0f;          // Enemy speed
    public float turningSpeed = 240f;   // Enemy turning speed
    public float acceleration = 15f;    // Enemy acceleration
    public float predictionTime = 0.7f;   // How far ahead of the player the enemy tries to predict movement

    // Detecting the Player
    public bool isListening = false;    // if true enemy will raycast to try to "see" the player
    public bool isAttacking = false;    // if true enemy will chase/attack the player
    public int detectionRange = 30;     // Within this range the enemy will raycast to try to "see" the player
    public int closeRange = 10;         // Within this range the enemy will immediately find the player even if behind it
    public int chaseDuration = 5;      // How long (seconds) enemy targets the player before needing to check if it can still see the player
    private float timeLastSighted;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed + (LevelState.currentDifficulty * 2);       //scales AI speed to difficulty
        agent.angularSpeed = turningSpeed;                              //scale these too??
        agent.acceleration = acceleration;                              //scale these too??
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
