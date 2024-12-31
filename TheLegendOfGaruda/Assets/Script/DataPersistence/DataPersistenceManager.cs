using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class DataPersistenceManager : MonoBehaviour
{
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
            Debug.Log("Found more than one data persistence manager in this scene");
        }
        instance = this;
    }

    private void Start()
    {
        // Application.dataPath bakal ngasih direktori app sesuai sama OS yang dipake user
        this.dataHandler = new FileDataHandler(Application.dataPath, fileName);
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

        // kalo gaada data buat di load bikin new game
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        // push data yang di load ke script lain yang butuh
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // pass data ke script yang lain jadi bisa di update
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
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
}
