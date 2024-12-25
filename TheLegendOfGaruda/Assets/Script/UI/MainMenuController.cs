using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string nextScene = "TutorialScene";
    public void OnStartClick()
    {
        SceneManager.LoadScene(nextScene);
    }
}
