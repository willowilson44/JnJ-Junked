using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI

public class LevelFinish : MonoBehaviour
{
    public GameObject endLevelPanel;
    public Button next;
    public Button menu;
    private TextMeshProUGUI killsText;
    private TextMeshProUGUI deathsText;
    private TextMeshProUGUI upgradesText;

    // Start is called before the first frame update
    void Start()
    {
        endLevelPanel = GameObject.Find("GUI/Canvas/Level Finish");
        endLevelPanel.SetActive(false);

        next.onClick.AddListener(() => nextClicked());
        menu.onClick.AddListener(() => menuClicked());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None; // Frees the cursor
            Cursor.visible = true; // Shows the cursor

            Debug.Log("End Level panel activated");
            endLevelPanel.SetActive(true);
            

            if (LevelState.currentLevel + 1 >= GameSettings.numLevels)
            {
                next.interactable = false;
            }

            if (endLevelPanel != null)
            {
                killsText = endLevelPanel.transform.Find("Kills").GetComponent<TextMeshProUGUI>();
                deathsText = endLevelPanel.transform.Find("Deaths").GetComponent<TextMeshProUGUI>();
                upgradesText = endLevelPanel.transform.Find("Upgrades").GetComponent<TextMeshProUGUI>();

                killsText.text = "Kills: " + LevelState.kills;
                deathsText.text = "Deaths: " + LevelState.deaths;
                upgradesText.text = "Upgrades: " + LevelState.newUpgrades;
            }
            else
            {
                Debug.LogError("Panel not found");
            }

            other.gameObject.SetActive(false);
            GameSettings.levelsCompleted[LevelState.currentLevel][LevelState.currentDifficulty] = true;
            GameSettings.SaveGameState();
        }
    }

    private void nextClicked()
    {
        LevelState.ResetGameState();
        SceneManager.LoadScene(GameSettings.levelNames[LevelState.currentLevel + 1]);
    }

    private void menuClicked()
    {
        LevelState.ResetGameState();
        SceneManager.LoadScene("Main Menu");
    }

    public void EndLevel()
    {
        GameObject player = ReferenceManager.instance.player;
        Cursor.lockState = CursorLockMode.None; // Frees the cursor
        Cursor.visible = true; // Shows the cursor

        Debug.Log("End Level panel activated");
        endLevelPanel.SetActive(true);


        if (LevelState.currentLevel + 1 >= GameSettings.numLevels)
        {
            next.interactable = false;
        }

        if (endLevelPanel != null)
        {
            killsText = endLevelPanel.transform.Find("Kills").GetComponent<TextMeshProUGUI>();
            deathsText = endLevelPanel.transform.Find("Deaths").GetComponent<TextMeshProUGUI>();
            upgradesText = endLevelPanel.transform.Find("Upgrades").GetComponent<TextMeshProUGUI>();

            killsText.text = "Kills: " + LevelState.kills;
            deathsText.text = "Deaths: " + LevelState.deaths;
            upgradesText.text = "Upgrades: " + LevelState.newUpgrades;
        }
        else
        {
            Debug.LogError("Panel not found");
        }

        player.gameObject.SetActive(false);
        GameSettings.levelsCompleted[LevelState.currentLevel][LevelState.currentDifficulty] = true;
        GameSettings.SaveGameState();
    }
}
