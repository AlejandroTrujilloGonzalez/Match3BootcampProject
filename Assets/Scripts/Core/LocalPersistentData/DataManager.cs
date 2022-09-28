using UnityEngine;
using System.IO;

public class DataManager
{
    public static string directory = "/SaveData/";
    public static string fileName = "PlayerData.json";

    public static void Save()
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(DataController.Instance.data, true);
        File.WriteAllText(dir + fileName, json);
    }

    public static PlayerData Load()
    {
        string dir = Application.persistentDataPath + directory + fileName;
        PlayerData data = DataController.Instance.data;

        if (File.Exists(dir)){
            string json = File.ReadAllText(dir);
            data = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.Log("Save file doesn't exist");
        }

        return data;
    }
}
