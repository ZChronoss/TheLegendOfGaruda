using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOver;
    private PanelFader panelFader;

    public void GameOver()
    {
        panelFader = GetComponent<PanelFader>();
        panelFader.Fade();
    }

    public void Retry()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
