using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour, IDataPersistence
{
    [Header("Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button continueGameButton;

    [Header("Audio")]
    [SerializeField] AudioSource btnPressedSFX;

    [Header("Fade Out Effect")]
    [SerializeField] GameObject fadeOut;

    private string playerScene;

    private void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnStartClick()
    {
        DisableMenuButtons();
        StartCoroutine(StartBtnCoroutine());
    }

    private IEnumerator StartBtnCoroutine()
    {
        btnPressedSFX.Play();
        fadeOut.SetActive(true);

        yield return new WaitForSeconds(1.7f);

        // bikin new game
        DataPersistenceManager.instance.NewGame();

        // load scene game nya
        // bakal save game karena OnSceneUnloaded di DataPersistenceManager
        SceneManager.LoadSceneAsync("IntroductionScene"); // <- sesuain sama level pertama
    }

    public void OnContinueClick()
    {
        DisableMenuButtons();
        StartCoroutine(ContinueBtnPressed());
        
    }

    private IEnumerator ContinueBtnPressed()
    {
        btnPressedSFX.Play();
        fadeOut.SetActive(true);

        yield return new WaitForSeconds(1.7f);

        // save game sebelum load scene baru(?)(ini gw kurang paham)
        DataPersistenceManager.instance.SaveGame();

        // load game yang ada di gamedata
        // bakal load game karena OnSceneLoaded di DataPersistenceManager
        SceneManager.LoadSceneAsync(playerScene); // <- harus di test, belom coba
    }

    // Disable button biar gabisa dipencet2 lagi pas lagi load scene
    private void DisableMenuButtons()
    {
        startButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void LoadData(GameData data)
    {
        this.playerScene = data.playerScene;
    }

    public void SaveData(GameData data)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        data.playerScene = SceneManager.GetActiveScene().name;
    }
}
