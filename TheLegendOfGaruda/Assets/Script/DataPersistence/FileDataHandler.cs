using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // Pake Path.Combine buat ngatasin kalo misalnya game dijalanin di OS yang beda beda
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                // load data dari file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize data dari JSON ke C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // Pake Path.Combine buat ngatasin kalo misalnya game dijalanin di OS yang beda beda
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // bikin directory buat write file kalo belom ada
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize data yang ada di C# object ke JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // write data ke file
            // using itu buat pastiin koneksi ke file itu diputusin kalo udah kelar write/read file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
