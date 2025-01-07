using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOver;

    public void GameOver()
    {
        gameOver.SetActive(true);
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
