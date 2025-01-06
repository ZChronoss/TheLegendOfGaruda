using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public InputActionAsset inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        inputActions.Disable();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        inputActions.Enable();
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        inputActions.Enable();
        SceneManager.LoadScene("MainMenu");
    }
}
