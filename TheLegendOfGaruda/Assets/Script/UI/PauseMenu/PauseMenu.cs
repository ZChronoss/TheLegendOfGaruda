using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public GameObject pauseMenu;
    public InputActionAsset inputActions;
    [SerializeField] private AudioClip buttonPressedSFX;

    public void PauseGame()
    {
        SFXManager.instance.PlaySFXClip(buttonPressedSFX, transform, 1f);
        gameObject.SetActive(true);
        inputActions.Disable();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        SFXManager.instance.PlaySFXClip(buttonPressedSFX, transform, 1f);
        gameObject.SetActive(false);
        inputActions.Enable();
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        SFXManager.instance.PlaySFXClip(buttonPressedSFX, transform, 1f);
        Time.timeScale = 1f;
        inputActions.Enable();
        SceneManager.LoadScene("MainMenu");
    }
}
