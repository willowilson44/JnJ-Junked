using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Required for UI
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    private Button[,] LevelButtons; 
    public Button devModeButton;
    public Button StartButton;
    public Button QuitButton;
    public Button OptionsButton;

    public bool developerMode = false;
    private Button currentHighlightedButton;
    private ColorBlock normalColors = ColorBlock.defaultColorBlock;
    private ColorBlock devModeButtonNormalColors;


    void Start()
    {
        GameSettings.LoadGameState();

        devModeButtonNormalColors = devModeButton.colors;

        LevelButtons = new Button[GameSettings.numLevels, GameSettings.numDifficulties];

        // Assign the buttons from the scene to the LevelButtons array
        for (int i = 0; i < LevelButtons.GetLength(0); i++) // Loop over levels
        {
            for (int j = 0; j < LevelButtons.GetLength(1); j++) // Loop over difficulties
            {
                string buttonPath = "Canvas/Menu/Difficulty/Level " + i + "/Difficulty " + j;
                Transform buttonTransform = GameObject.Find(buttonPath).transform;

                // Assigning the found button to the 2D array
                LevelButtons[i, j] = buttonTransform.GetComponent<Button>();
            }
        }


        // Add Onclick Listeners to all level buttons
        for (int i = 0; i < LevelButtons.GetLength(0); i++) // Loop over levels
        {
            for (int j = 0; j < LevelButtons.GetLength(1); j++) // Loop over difficulties
            {
                int level = i; // local variable is captured in the closure, so it needs to be a local variable
                int difficulty = j; // same reason as above

                LevelButtons[i, j].onClick.AddListener(() => OnLevelButtonClicked(level, difficulty, LevelButtons[level, difficulty]));
            }
        }

        devModeButton.onClick.AddListener(() => OnDevModeClicked());
        StartButton.onClick.AddListener(() => OnStartClicked());
        QuitButton.onClick.AddListener(() => OnQuitClicked());
        OptionsButton.onClick.AddListener(() => OnOptionsClicked());


        LockAll();
        UnlockNecessary();
    }

    private void LockAll()
    {
        // Make all level buttons uninteractable
        for (int i = 0; i < LevelButtons.GetLength(0); i++)
        {
            for (int j = 0; j < LevelButtons.GetLength(1); j++)
            {
                LevelButtons[i, j].interactable = false;
            }
        }
    }

    private void UnlockNecessary()
    {
        // Unlock appropriate levels
        LevelButtons[0, 0].interactable = true;

        for (int level = 0; level < LevelButtons.GetLength(0); level++)
        {
            for (int difficulty = 0; difficulty < LevelButtons.GetLength(1); difficulty++)
            {
                if (GameSettings.levelsCompleted[level][difficulty] == true)
                {
                    //Unlock next difficulty in this level
                    if (difficulty+1 < LevelButtons.GetLength(1))
                    {
                        LevelButtons[level, difficulty+1].interactable = true;
                    }
                    //Unlock this difficulty in next level
                    if (level+1 < LevelButtons.GetLength(0))
                    {
                        LevelButtons[level+1, difficulty].interactable = true;
                    }
                }
            }
        }
    }


    private void UnlockAll()
    {
        for (int i = 0; i < LevelButtons.GetLength(0); i++)
        {
            for (int j = 0; j < LevelButtons.GetLength(1); j++)
            {
                LevelButtons[i, j].interactable = true;
            }
        }
    }

    void OnLevelButtonClicked(int level, int difficulty, Button clickedButton)
    {
        // Handle the button click event here
        Debug.Log($"Level: {level}, Difficulty: {difficulty} button clicked!");
        LevelState.currentLevel = level;
        LevelState.currentDifficulty = difficulty;


        if (currentHighlightedButton != null && currentHighlightedButton != clickedButton)
        {
            // if there is a previous highlighted button, revert its colors to normal
            currentHighlightedButton.colors = normalColors;
        }

        if (currentHighlightedButton != clickedButton)
        {
            // Save the current color block of the clicked button before changing it
            normalColors = clickedButton.colors;

            ColorBlock selectedColors = clickedButton.colors;
            selectedColors.normalColor = selectedColors.selectedColor;
            clickedButton.colors = selectedColors;

            // Update the reference to the currently highlighted button
            currentHighlightedButton = clickedButton;
        }
    }

    private GameObject GetHighlightImage(Button button)
    {
        // Example of getting highlight image, replace with actual way to access the highlight image
        return button.transform.Find("HighlightImage").gameObject;
    }

    void OnDevModeClicked()
    {
        developerMode = !developerMode; // Toggle developer mode
        if (developerMode)
        {
            Debug.Log("Devmode enabled");

            ColorBlock selectedColors = devModeButton.colors;
            selectedColors.normalColor = selectedColors.selectedColor;
            devModeButton.colors = selectedColors;

            UnlockAll();
        }
        else
        {
            Debug.Log("Devmode disabled");
            devModeButton.colors = devModeButtonNormalColors;
            EventSystem.current.SetSelectedGameObject(null); // Clear the currently selected object

            LockAll();
            UnlockNecessary();
        }
    }

    void OnStartClicked()
    {
        string sceneName = "";
        switch (LevelState.currentLevel)
        {
            case 0:
                sceneName = GameSettings.levelNames[0]; // replace with your scene name for level 0
                break;
            case 1:
                sceneName = GameSettings.levelNames[1]; // replace with your scene name for level 1
                break;
            case 2:
                sceneName = GameSettings.levelNames[2]; // replace with your scene name for level 2
                break;
            // Add more cases as needed for additional levels.
            default:
                Debug.LogError("Invalid level selected!");
                return; // exit the method without loading a scene.
        }

        Debug.Log("Loading Level: " + LevelState.currentLevel + ", Scene Name is: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    void OnQuitClicked()
    {
        GameSettings.SaveGameState();
        Debug.Log("Quitting");
    }
    void OnOptionsClicked()
    {

    }
}