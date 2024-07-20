using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string path = "";
    private string fileName = "";

    public FileDataHandler(string dataPath, string dataFileName)
    {
        this.path = dataPath;
        this.fileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(path, fileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception exception)
            {
                Debug.LogError("Error occured when trying to load data to file: " + fullPath + "/n" + exception);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(path, fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                    //Debug.Log("Data saved in the following path: " + fullPath);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "/n" + exception);
        }
    }

    public string GetFullPathToSaveFile()
    {
        string fullPath = Path.Combine(path, fileName);
        return fullPath;
    }
}