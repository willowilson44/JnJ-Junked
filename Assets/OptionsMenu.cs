using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    private InputMaster controls;
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public GameObject introSkipMenuUI;
    public Button options;
    public Button back;
    public Slider volume;
    public Slider mouseSensitivity;
    public Toggle mouseInvertY;
    public Slider gamepadSensitivity;
    public Toggle gamepadInvertY;
    //private bool inOptions = false;

    [Header("Settings")]
    public float defaultMouseSensitivity = 1.0f;
    public float defaultGamepadSensitivity = 1.0f;

    private void Awake()
    {

    }

    private void Start()
    {
        optionsMenuUI.SetActive(false);

        options.onClick.AddListener(() => Options());
        back.onClick.AddListener(() => Back());

        volume.onValueChanged.AddListener(ChangeVolume);

        LoadSettings();
    }


    void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }



    public void Options()
    {
        //inOptions = true;
        mainMenuUI.SetActive(false);
        introSkipMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void Back()
    {
        //inOptions = false;
        SaveSettings();
        optionsMenuUI.SetActive(false);
        introSkipMenuUI.SetActive(true);
        mainMenuUI.SetActive(true);
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
    }
}

