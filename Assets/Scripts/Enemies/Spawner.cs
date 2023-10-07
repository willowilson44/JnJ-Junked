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
    private int spawnedCount;
    private float startHeight;
    private float endHeight;
    private float positionOffsetX = 1.2f;
    private float positionOffsetZ = -1.7f;
    private float raiseTime = 2f;
    private bool isActive = true;
    public int spawnsAtOnce = 2;
    public int spawnWait = 12;
    public int spawnTotal = 3;
    public int activationRange = 120;
    float lastSpawnTime = 0;

    private Renderer trapdoorRenderer;
    private Renderer chevronRenderer;
    Material trapdoorMaterial;
    Material chevronMaterial;
    Color initialColor;
    Color transparentColor;

    // Start is called before the first frame update
    void Start()
    {
        
        spawner = this.gameObject;
        spawnedEnemies = new GameObject[spawnsAtOnce]; // Initialize the spawnedEnemies array
        player = ReferenceManager.instance.player;

        switch (enemyType)
        {
            case EnemyType.Scrapper:
                raiseTime *= 0.5f;
                startHeight = -0.3f;
                endHeight = 0.5f;
                positionOffsetX = positionOffsetX * 0.6f;
                positionOffsetZ = positionOffsetZ * 0.6f;
                break;

            case EnemyType.Guard:
                startHeight = -3f;
                endHeight = 1.6f;
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

        // Obtain reference to the Trapdoor's material
        trapdoorRenderer = spawner.transform.Find("Trapdoor").GetComponent<Renderer>();
        trapdoorMaterial = trapdoorRenderer.material;
        initialColor = trapdoorMaterial.color;
        transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);        // Prepare the transparent color (keeping the same RGB but with 0 alpha)

        spawnWait = spawnWait*(1-(LevelState.currentDifficulty/5));
        spawnTotal = spawnTotal+(LevelState.currentDifficulty*2);

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
                        spawnedCount++;
                        spawnedEnemies[i] = SpawnEnemy();
                        lastSpawnTime = Time.time; // Update the last spawn time


                    if (spawnedCount >= spawnTotal)
                    {
                        DeactivateSpawner();
                    }

                    break; // Exit the loop since we've spawned an enemy this frame
                    }
                }
            }

        

        
    }



    private void DeactivateSpawner()
    {
        Transform coverChild = transform.Find("Cover");
        if (coverChild != null)
        {
            coverChild.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Cover child not found in spawner.");
        }


        this.enabled = false;
    }

    private GameObject SpawnEnemy()
    {
        GameObject newEnemy;
        Vector3 offset = new Vector3(positionOffsetX, startHeight, positionOffsetZ); // Create the offset as a vector
        Vector3 startPos = spawner.transform.position + spawner.transform.TransformDirection(offset); // Apply the offset based on the spawner's orientation

        newEnemy = Instantiate(Resources.Load<GameObject>(enemyType.ToString()), startPos, spawner.transform.rotation);

        StartCoroutine(RaiseEnemy(newEnemy, startPos));

        return newEnemy;
    }


    IEnumerator RaiseEnemy(GameObject newEnemy, Vector3 startPos)
    {
        // Get the NavMeshAgent and Rigidbody components
        var navMeshAgent = newEnemy.GetComponent<NavMeshAgent>();
        var rigidbody = newEnemy.GetComponent<Rigidbody>();

        // Disable the components
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        if (rigidbody != null) rigidbody.isKinematic = true; // Making Rigidbody kinematic will stop it from being affected by physics


        float startTime = Time.time; // Save the start time

        // Calculate the start and end positions relative to the spawner's position
        Vector3 offsetEndPos = new Vector3(positionOffsetX, endHeight, positionOffsetZ); // Create the offset as a vector
        Vector3 endPos = spawner.transform.position + spawner.transform.TransformDirection(offsetEndPos); // Apply the offset based on the spawner's orientation

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
