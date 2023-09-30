using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public enum EnemyType { Scrapper, Guard, Dalek };
    public EnemyType enemyType;
    private GameObject spawner;
    private GameObject player;
    private GameObject[] spawnedEnemies;
    private float startHeight;
    private float endHeight;
    private float positionOffsetX = 1.2f;
    private float positionOffsetZ = -1.7f;
    private float raiseTime = 2f;
    public int spawnsAtOnce = 3;
    public int spawnWait = 15;
    public int activationRange = 80;
    float lastSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawner = this.gameObject;
        spawnedEnemies = new GameObject[spawnsAtOnce]; // Initialize the spawnedEnemies array
        player = ReferenceManager.instance.player;

        switch (enemyType)
        {
            case EnemyType.Scrapper:
                startHeight = -1.5f;
                endHeight = -0.2f;
                positionOffsetX = positionOffsetX * 0.6f;
                positionOffsetZ = positionOffsetZ * 0.6f;
                break;

            case EnemyType.Guard:
                startHeight = -3f;
                endHeight = 1f;
                break;

            case EnemyType.Dalek:
                startHeight = -4f;
                endHeight = -0f;
                positionOffsetX = positionOffsetX * 1.3f;
                positionOffsetZ = positionOffsetZ * 1.3f;
                break;

            default:
                Debug.LogError("Unrecognized enemy type!");
                break;
        }

        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within detection range
        if (distanceToPlayer >= activationRange)
        { 
            return; 
        }


        // If enough time has passed since the last spawn
        if (Time.time >= lastSpawnTime + spawnWait)
        {
            // Check each index in spawnedEnemies
            for (int i = 0; i < spawnedEnemies.Length; i++)
            {
                // If an index is null (empty), spawn a new enemy there
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies[i] = SpawnEnemy();
                    lastSpawnTime = Time.time; // Update the last spawn time
                    break; // Exit the loop since we've spawned an enemy this frame
                }
            }
        }
    }

    private GameObject SpawnEnemy()
    {
        GameObject newEnemy;
        Vector3 startPos = spawner.transform.position;
        startPos.x += positionOffsetX;
        startPos.y += startHeight;
        startPos.z += positionOffsetZ;

        newEnemy = Instantiate(Resources.Load<GameObject>(enemyType.ToString()), startPos, spawner.transform.rotation);

        StartCoroutine(RaiseEnemy(newEnemy, startPos));

        return newEnemy;
    }

    IEnumerator RaiseEnemy(GameObject newEnemy, Vector3 startPos)
    {
        // Obtain reference to the Trapdoor's material
        Renderer trapdoorRenderer = spawner.transform.Find("Trapdoor").GetComponent<Renderer>();
        Material trapdoorMaterial = trapdoorRenderer.material;
        Color initialColor = trapdoorMaterial.color;

        // Prepare the transparent color (keeping the same RGB but with 0 alpha)
        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Get the NavMeshAgent and Rigidbody components
        var navMeshAgent = newEnemy.GetComponent<NavMeshAgent>();
        var rigidbody = newEnemy.GetComponent<Rigidbody>();

        // Disable the components
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        if (rigidbody != null) rigidbody.isKinematic = true; // Making Rigidbody kinematic will stop it from being affected by physics


        float startTime = Time.time; // Save the start time

        // Calculate the start and end positions relative to the spawner's position
        Vector3 endPos = spawner.transform.position;
        endPos.x += positionOffsetX;
        endPos.y += endHeight;
        endPos.z += positionOffsetZ;

        while (Time.time < startTime + raiseTime)
        {
            float t = (Time.time - startTime) / raiseTime; // Normalized elapsed time
            newEnemy.transform.position = Vector3.Lerp(startPos, endPos, t);

            if (t <= 1f / 3f) // First third of the time
            {
                float transition = t * 3; // Normalized transition time between 0 and 1/3
                trapdoorMaterial.color = Color.Lerp(initialColor, transparentColor, transition);
            }
            else if (t <= 2f / 3f) // Second third of the time
            {
                // Keep it transparent
                trapdoorMaterial.color = transparentColor;
            }
            else // Final third of the time
            {
                float transition = (t - 2f / 3f) * 3; // Normalized transition time between 2/3 and 1
                trapdoorMaterial.color = Color.Lerp(transparentColor, initialColor, transition);
            }

            yield return null;
        }

        newEnemy.transform.position = endPos; // Ensure it's exactly at the end position when done
        trapdoorMaterial.color = initialColor;

        // Re-enable the components
        if (navMeshAgent != null) navMeshAgent.enabled = true;
        if (rigidbody != null) rigidbody.isKinematic = false; // Allow the Rigidbody to be affected by physics again
    }
}
