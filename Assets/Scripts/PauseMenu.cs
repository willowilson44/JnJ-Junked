
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private InputMaster controls;
    public GameObject pauseMenuUI; 
    public GameObject optionsMenuUI;
    public GameObject crosshair;
    public Button resume;
    public Button options;
    public Button menu;
    public Button back;
    public Slider volume;
    public Slider mouseSensitivity;
    public Toggle mouseInvertY;
    public Slider gamepadSensitivity;
    public Toggle gamepadInvertY;
    private bool isPaused = false;
    private bool inOptions = false;
    private CameraFollow mainCamera;

    [Header("Settings")]
    public float defaultMouseSensitivity = 1.0f;
    public float defaultGamepadSensitivity = 1.0f;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Pause.performed += _ =>
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                if (inOptions)
                {
                    Back();
                } else
                {
                    Resume();
                }
            }
        };

    }

    private void Start()
    {
        LockCursor();
        mainCamera = ReferenceManager.instance.mainCamera;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

        resume.onClick.AddListener(() => Resume());
        options.onClick.AddListener(() => Options());
        menu.onClick.AddListener(() => QuitGame());
        back.onClick.AddListener(() => Back());

        volume.onValueChanged.AddListener(ChangeVolume);
        mouseSensitivity.onValueChanged.AddListener(ChangeMouseSensitivity);
        mouseInvertY.onValueChanged.AddListener(ToggleMouseInvertY);
        gamepadSensitivity.onValueChanged.AddListener(ChangeGamepadSensitivity);
        gamepadInvertY.onValueChanged.AddListener(ToggleGamepadInvertY);

        LoadSettings();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


    void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }

    void ChangeMouseSensitivity(float value)
    {
        mainCamera.updateMouseLook(value);
    }

    void ToggleMouseInvertY(bool isInverted)
    {
        mainCamera.invertMouseLook(isInverted);
    }

    void ChangeGamepadSensitivity(float value)
    {
        mainCamera.updateGamepadLook(value);
    }

    void ToggleGamepadInvertY(bool isInverted)
    {
        mainCamera.invertGamepadLook(isInverted);
    }

    void Pause()
    {
        UnlockCursor();
        crosshair.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // This stops game time, effectively pausing all in-game actions.
        isPaused = true;
    }

    public void Resume()
    {
        LockCursor();
        crosshair.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // This resumes the game time.
        isPaused = false;
    }

    public void Options()
    {
        inOptions = true;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        isPaused = false;
        GameSettings.SaveGameState();
        Time.timeScale = 1f; // Ensure time is resumed before changing scenes.
        SceneManager.LoadScene("Main Menu"); // Replace with your main menu scene name.
    }

    public void Back()
    {
        inOptions = false;
        SaveSettings();
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity.value);
        PlayerPrefs.SetInt("MouseInvertY", mouseInvertY.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("GamepadSensitivity", gamepadSensitivity.value);
        PlayerPrefs.SetInt("GamepadInvertY", gamepadInvertY.isOn ? 1 : 0);
        PlayerPrefs.Save();  // Writes all modified settings to disk
    }

    void LoadSettings()
    {
        volume.value = PlayerPrefs.GetFloat("Volume", 0.5f); // 1f is a default value if "Volume" hasn't been set yet.
        mouseSensitivity.value = PlayerPrefs.GetFloat("MouseSensitivity", defaultMouseSensitivity);
        mouseInvertY.isOn = PlayerPrefs.GetInt("MouseInvertY", 0) == 1;
        gamepadSensitivity.value = PlayerPrefs.GetFloat("GamepadSensitivity", defaultGamepadSensitivity);
        gamepadInvertY.isOn = PlayerPrefs.GetInt("GamepadInvertY", 0) == 1;

        // Apply these settings immediately after loading (e.g., set the game volume, apply the mouse sensitivity, etc.)
        ChangeVolume(volume.value);
        ChangeMouseSensitivity(mouseSensitivity.value);
        ToggleMouseInvertY(mouseInvertY.isOn);
        ChangeGamepadSensitivity(gamepadSensitivity.value);
        ToggleGamepadInvertY(gamepadInvertY.isOn);
    }


    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        Cursor.visible = false; // Hides the cursor
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Frees the cursor
        Cursor.visible = true; // Shows the cursor
    }
}
