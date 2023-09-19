using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class GuardBehaviour : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;
    PlayerPositionUpdate playerPosition;

    public float speed = 8.0f; // Enemy speed
    public float turningSpeed = 240f; // Enemy turning speed
    public float acceleration = 15f; // Enemy turning speed
    public float predictionTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerPosition = player.GetComponent<PlayerPositionUpdate>();

        agent.speed = speed;
        agent.angularSpeed = turningSpeed;
        agent.acceleration = acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(player.transform.position);
        Vector3 playerVelocity = playerPosition.velocity;
        Vector3 futurePosition = playerPosition.position + playerVelocity/40 * predictionTime;

        // Now move towards the predicted future position
        //Vector3 directionToFuturePosition = futurePosition - transform.position;
        //.Normalize();
        //transform.position += directionToFuturePosition * speed * Time.deltaTime;
        //agent.SetDestination(directionToFuturePosition);

        // Debugging logs
        Debug.Log("Player Velocity: " + playerVelocity);
        Debug.Log("Player Position: " + playerPosition.position);
        Debug.Log("Predicted Future Position: " + futurePosition);


        agent.SetDestination(futurePosition);
    }
}
