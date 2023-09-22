using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstantiateAI : MonoBehaviour
{
    string currentEnemy;
    int currentEnemyIndex = 0;

    void Start()
    {
        currentEnemy = ReferenceManager.instance.enemies[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit info))
            {
                InstantiateEnemy(info.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Increment the index and wrap it around if it reaches the end of the array
            currentEnemyIndex = (currentEnemyIndex + 1) % ReferenceManager.instance.enemies.Length;
            currentEnemy = ReferenceManager.instance.enemies[currentEnemyIndex];

            TMP_Text enemyText = GameObject.Find("GUI/Canvas/Enemy to Add").GetComponent<TMP_Text>();
            enemyText.text = "Enemy: " + currentEnemy;
        }
    }

    void InstantiateEnemy(Vector3 position)
    {
        GameObject player = ReferenceManager.instance.player;

        Vector3 directionToPlayer = player.transform.position - position;
        directionToPlayer.y = 0;  // This ensures that the enemy does not tilt upwards/downwards and only rotates around the y-axis

        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        Instantiate(Resources.Load<GameObject>(currentEnemy), position, rotation);
    }
}
