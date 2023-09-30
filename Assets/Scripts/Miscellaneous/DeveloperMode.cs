using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeveloperMode : MonoBehaviour
{
    private InputMaster controls;
    string currentEnemy;
    int currentEnemyIndex = 0;

    void Awake()
    {
        controls = new InputMaster();

        controls.Player.ToggleDevMode.performed += _ => {
            ToggleDevMode();
        };

        controls.Player.ToggleEnemy.performed += _ => {
            ToggleEnemy();
        };

        controls.Player.InstantiateEnemy.performed += context => {
            InstantiateEnemyStart(context);
        };
    }

    private void Start()
    {
        currentEnemy = ReferenceManager.instance.enemies[0];

    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }


    private void InstantiateEnemyStart(InputAction.CallbackContext context)
    {
        if (LevelState.devMode == true)
        {
            var control = context.control;
            Debug.Log($"Action Performed by " + control.device);
            Ray ray;

            if (control.device is Mouse)
            {
                // Perform raycast from mouse position if control device is a mouse
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                // Perform raycast from screen center if control device is not a mouse
                var screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                ray = Camera.main.ScreenPointToRay(screenCenter);
            }

            // Perform the raycast and instantiate enemy if hit occurs
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                InstantiateEnemy(info.point);
            }
        }
    }


    private void ToggleEnemy()
    {
        // Increment the index and wrap it around if it reaches the end of the array
        currentEnemyIndex = (currentEnemyIndex + 1) % ReferenceManager.instance.enemies.Length;
        currentEnemy = ReferenceManager.instance.enemies[currentEnemyIndex];

        TMP_Text enemyText = GameObject.Find("GUI/Canvas/Enemy to Add").GetComponent<TMP_Text>();
        enemyText.text = "Enemy: " + currentEnemy;

    }

    void InstantiateEnemy(Vector3 position)
    {
        GameObject player = ReferenceManager.instance.player;

        Vector3 directionToPlayer = player.transform.position - position;
        directionToPlayer.y = 0;  // This ensures that the enemy does not tilt upwards/downwards and only rotates around the y-axis

        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        Instantiate(Resources.Load<GameObject>(currentEnemy), position, rotation);
    }


    private void ToggleDevMode()
    {
        if (LevelState.devMode == true)
        {
            LevelState.devMode = false;
            Debug.Log("Dev Mode Disabled");
        }
        else
        {
            LevelState.devMode = true;
            Debug.Log("Dev Mode Enabled");
        }

        PlayerState.UpdateEnergy(PlayerState.currentMax);
    }
}
