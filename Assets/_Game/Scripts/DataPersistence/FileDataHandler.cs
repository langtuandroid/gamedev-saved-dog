using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{
    private readonly string dataDirPath = "";
    private readonly string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (!File.Exists(fullPath))
        {
            return null;
        }

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

            loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
        } catch (Exception e)
        {
            Debug.LogError("Error load game: " + fullPath + "\n" + e);
        }
        return loadedData;
    }
    
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(data);

            File.WriteAllText(fullPath, dataToStore);
        } catch (Exception e)
        {
            Debug.LogError("Error save game: " + fullPath + "\n" + e);
        }
    }
}
