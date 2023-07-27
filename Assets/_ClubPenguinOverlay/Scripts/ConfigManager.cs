using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NaughtyAttributes;
public class ConfigManager : Singleton<ConfigManager>
{
    [SerializeField] private string configFileName = "config.json";
    private Config config;

    private bool init;

    public Config Config
    {
        get
        {
            if(init == false)
            {
                config = Config.LoadFromFile(configFileName);
                init = true;
            }
            return config;
        }
    }

    //[Button]
    //private void SaveConfigFile()
    //{
    //    Debug.Log("SaveConfigFile");
    //    config.SaveToFile(configFileName);
    //}
}

[System.Serializable]
public class Config
{
    //Twitch settings
    [SerializeField] private string user;
    [SerializeField] private string oAuth; //get OAuth from https://twitchapps.com/tmi
    [SerializeField] private string channel;

    //Game Settings
    [SerializeField] private int maxPenguinCount = 120;
    [SerializeField] private bool joinCommandToJoin = true;
    [SerializeField] private float penguinTimeout = 600;
    [SerializeField] private bool showBadges = true;

    [SerializeField] private int maxSnowballCount = 10;


    [SerializeField] private int targetFrameRate = 30;
    [SerializeField] private int screenHeight = 1920;


    public string User => user;
    public string OAuth => oAuth;
    public string Channel => channel;
    public int MaxPenguinCount => maxPenguinCount;
    public bool JoinCommandToJoin => joinCommandToJoin;
    public float PenguinTimeout => penguinTimeout;
    public bool ShowBadges => showBadges;
    public int MaxSnowballCount => maxSnowballCount;
    public int TargetFrameRate => targetFrameRate;
    public int ScreenHeight => screenHeight;


    public string SerializeToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // Deserialize the object from JSON
    public static Config DeserializeFromJson(string json)
    {
        return JsonUtility.FromJson<Config>(json);
    }

    // Load the serialized object from a file in StreamingAssets
    public static Config LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return DeserializeFromJson(json);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return new Config();
        }
    }

    // Save the serialized object to a file
    public void SaveToFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string json = SerializeToJson();
        File.WriteAllText(filePath, json);
    }

}
