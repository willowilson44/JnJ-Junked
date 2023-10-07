using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Notifications : MonoBehaviour
{
    [SerializeField] private float defaultDisplayDuration = 5.0f;  // Default duration for which a notification will be displayed

    private TextMeshProUGUI textComponent;
    private Image image;

    private void Awake()
    {
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
        // Initially, set the text component to inactive
        textComponent.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (LevelState.firstLoad)
        {
            StartCoroutine(InitialNotificationDelay());
            LevelState.firstLoad = false;
        }
    }

    private IEnumerator InitialNotificationDelay()
    {
        yield return new WaitForSeconds(0.8f);  // Wait for 1 second (or however long you want)
        DisplayNotification("With no weapons at your disposal, confrontation is a dangerous game. \n\nScavenge for upgrades. The path to escape is paved with discarded parts...");
    }

    public void DisplayNotification(string message)
    {
        DisplayNotification(message, defaultDisplayDuration);
    }

    public void DisplayNotification(string message, float duration)
    {
        PauseGame();
        textComponent.text = message;
        textComponent.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        StartCoroutine(HideAfterDelay(duration));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + delay)
        {
            yield return null;
        }
        HideNotification();
    }


    private void HideNotification()
    {
        textComponent.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
