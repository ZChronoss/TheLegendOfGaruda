using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found more than one data persistence manager in this scene. Destroying the new one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Application.dataPath bakal ngasih direktori app sesuai sama OS yang dipake user
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    // Ini scene manager subscribe ke OnSceneLoaded biar jalan
    // OnEnable itu mirip kek start tp jalannya setelah awake dan sebelum start
    // refer ke sini: https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // OnDisable perlu buat unsubscribe semuanya pas gamenya udah di terminate
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // load saved data dari file pake data handler
        this.gameData = dataHandler.Load();

        // MARK - DEBUGGING PURPOSE
        // start new game kalo data null
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        // kalo gaada data buat di load, return
        if(this.gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before data can be loaded.");
            return;
        }

        // push data yang di load ke script lain yang butuh
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // kalo gaada data buat di save, kasih warning log
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be started before data can be saved");
            return;
        }

        // pass data ke script yang lain jadi bisa di update
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // save data ke file pake data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        // ini di tutorial kalo keluar app bakal save game.
        //SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
}
