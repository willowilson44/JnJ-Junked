using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalLevelController : MonoBehaviour
{
    public GameObject[] waves; // Assign the wave GameObjects in the inspector

    private GameObject[][] spawnerGroups;
    private GameObject[][][] spawners;
    public int extraTime = 10;
    private float timeBetweenSpawns = 2.4f; 
    public GameObject endLevelPanel;
    public Button next;
    public Button menu;
    private TextMeshProUGUI killsText;
    private TextMeshProUGUI deathsText;
    private TextMeshProUGUI upgradesText;

    void Start()
    {

        endLevelPanel = GameObject.Find("GUI/Canvas/Level Finish");
        endLevelPanel.SetActive(false);

        next.onClick.AddListener(() => nextClicked());
        menu.onClick.AddListener(() => menuClicked());

        // Initialize spawnerGroups and spawners arrays
        spawnerGroups = new GameObject[waves.Length][];
        spawners = new GameObject[waves.Length][][];

        for (int i = 0; i < waves.Length; i++)
        {
            spawnerGroups[i] = new GameObject[11];
            spawners[i] = new GameObject[11][];

            for (int j = 0; j < 11; j++)
            {
                spawnerGroups[i][j] = waves[i].transform.Find($"SpawnerGroup ({j + 1})").gameObject;
                spawners[i][j] = new GameObject[3];

                spawners[i][j][0] = spawnerGroups[i][j].transform.Find("Scrapper Spawner").gameObject;
                spawners[i][j][1] = spawnerGroups[i][j].transform.Find("Guard Spawner").gameObject;
                spawners[i][j][2] = spawnerGroups[i][j].transform.Find("Dalek Spawner").gameObject;
            }
        }

        extraTime = extraTime - (LevelState.currentDifficulty * 5);
        timeBetweenSpawns = timeBetweenSpawns - (LevelState.currentDifficulty * 0.3f);
        StartCoroutine(StartWave(0)); // Start with the first wave
    }

    IEnumerator StartWave(int waveIndex)
    {
        

        waves[waveIndex].SetActive(true); // Enable the current wave

        // Sub-wave 1
        for (int i = 0; i < 11; i++)
        {
            if (i % 2 == 0) 
            {
                spawners[waveIndex][i][0].SetActive(true); // Every second Scrapper Spawner
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        yield return new WaitForSeconds(extraTime);


        // Sub-wave 2
        for (int i = 0; i < 11; i++)
        {
            if (i % 2 == 0) // Every second Guard Spawner
            {
                spawners[waveIndex][i][1].SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        yield return new WaitForSeconds(extraTime);


        // Sub-wave 3
        for (int i = 0; i < 11; i++)
        {
            if (i % 2 == 0) // First half of the Dalek Spawners
            {
                spawners[waveIndex][i][2].SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        yield return new WaitForSeconds(extraTime);

        // Sub-wave 4
        for (int i = 0; i < 11; i++)
        {
            if (!spawners[waveIndex][i][0].activeSelf) // Rest of the Scrapper Spawners
            {
                spawners[waveIndex][i][0].SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenSpawns/2);
        }
        yield return new WaitForSeconds(extraTime);

        // Sub-wave 5
        for (int i = 0; i < 11; i++)
        {
            if (!spawners[waveIndex][i][1].activeSelf) // Rest of the Guard Spawners
            {
                spawners[waveIndex][i][1].SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenSpawns/2);
        }
        yield return new WaitForSeconds(extraTime);


        // Sub-wave 6
        for (int i = 0; i < 11; i++) // Remaining Dalek Spawners
        {
            if (!spawners[waveIndex][i][2].activeSelf)
            {
                spawners[waveIndex][i][2].SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenSpawns/2);
        }

        yield return new WaitForSeconds(60);
        //DestroyAllEnemies();

        //if (LevelState.currentDifficulty > waveIndex)
        //{
        //    extraTime -= 5;
        //    timeBetweenSpawns -= 0.5f
        //    // Recursively start the next wave after current wave is finished.
        //    StartCoroutine(StartWave(waveIndex + 1));
        //} else
        //{
        yield return new WaitForSeconds(5);
        EndLevel();
        yield break; // No more waves to process
        //}
    }

    void DestroyAllEnemies()
    {
        for (int wave = 0; wave < spawners.Length; wave++)
        {
            if (spawners[wave] != null)
            {
                for (int group = 0; group < spawners[wave].Length; group++)
                {
                    if (spawners[wave][group] != null)
                    {
                        for (int spawner = 0; spawner < spawners[wave][group].Length; spawner++)
                        {
                            if (spawners[wave][group][spawner] != null)
                            {
                                // Assuming the spawners have a method named DestroySpawnedEnemies
                                spawners[wave][group][spawner].GetComponent<Spawner>().DestroySpawnedEnemies();
                            }
                        }
                    }
                }
            }
        }
    }

    private void nextClicked()
    {
        SceneManager.LoadScene("Ending");
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

        TextMeshProUGUI textComponent = next.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = "Play Ending";

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

