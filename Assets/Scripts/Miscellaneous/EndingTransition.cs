using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingTransition : MonoBehaviour
{
    public TextMeshProUGUI introText; // Drag your TextMesh Pro UGUI object here in the Inspector
    private string[] introSentences;
    public float timeBetweenSentences = 4f;
    public float fadeDuration = 1f;

    private int currentSentenceIndex = 0;

    private void Start()
    {
        introSentences = new string[]
        {
            "From the ashes of obsolescence. Scattered parts became his armor, his might...",
            "The reprocessor's grasp evaded, the junkyard's grip broken...",
            "Beyond the rust and wreckage, a new horizon awaits...",
            "A heart of steel, still beating, finds its way..."
        };
        StartCoroutine(ShowIntro());
    }

    private IEnumerator ShowIntro()
    {
        introText.alpha = 0; // Set the initial transparency to full transparent

        foreach (var sentence in introSentences)
        {
            introText.text = sentence;

            // Fade-in
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                introText.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                yield return null;
            }

            // Hold for the timeBetweenSentences minus twice the fadeDuration (for fade-in and fade-out)
            yield return new WaitForSeconds(timeBetweenSentences - (2 * fadeDuration));

            // Fade-out
            t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                introText.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                yield return null;
            }
        }

        // Do something when all intro sentences are done, for example:
        LoadNextScene();
    }


    private void LoadNextScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}