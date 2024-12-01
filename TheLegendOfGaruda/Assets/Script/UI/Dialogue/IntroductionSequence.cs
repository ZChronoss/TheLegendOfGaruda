using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public GameObject blackPanel;
    public string[] storyLines;
    public float textSpeed = 2f;
    public float linePause = 2f;
    public string nextSceneName;

    private void Start()
    {
        // Start the black screen intro sequence
        StartCoroutine(DisplayStory());
    }

    private IEnumerator DisplayStory()
    {
        // Ensure the black panel is visible
        blackPanel.SetActive(true);

        // Loop through all the story lines
        foreach (string line in storyLines)
        {
            storyText.text = line;
            yield return StartCoroutine(FadeTextIn(storyText));
            yield return new WaitForSeconds(linePause);
            yield return StartCoroutine(FadeTextOut(storyText));
        }

        // Transition to the game scene
        StartCoroutine(TransitionToGameScene());
    }

    private IEnumerator FadeTextIn(TextMeshProUGUI text)
    {
        Color color = text.color;
        color.a = 0; // Start fully transparent
        text.color = color;

        // Gradually increase alpha
        while (color.a < 1)
        {
            color.a += Time.deltaTime / textSpeed;
            text.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeTextOut(TextMeshProUGUI text)
    {
        Color color = text.color;
        color.a = 1; // Start fully opaque

        // Gradually decrease alpha
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / textSpeed;
            text.color = color;
            yield return null;
        }
    }

    private IEnumerator TransitionToGameScene()
    {
        // Optionally fade out the black panel
        Image panelImage = blackPanel.GetComponent<Image>();
        Color panelColor = panelImage.color;

        while (panelColor.a > 0)
        {
            panelColor.a -= Time.deltaTime / textSpeed;
            panelImage.color = panelColor;
            yield return null;
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
