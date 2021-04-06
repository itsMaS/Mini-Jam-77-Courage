using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-500)]
public class GameManager : MonoBehaviour
{
    public struct userAttributes
    {
        // Optionally declare variables for any custom user attributes:
        public bool expansionFlag;
    }

    public struct appAttributes
    {
        // Optionally declare variables for any custom app attributes:
        public int level;
        public int score;
        public string appVersion;
    }

    public static GameManager Instance;
    private void Awake()
    {
        if(Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }

    public GameConfig config;
    private void Initialize()
    {
        Instance = this;
        LoadConfig();
    }
    private void LoadConfig()
    {
        ConfigManager.FetchCompleted += ConfigLoaded;
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
        Debug.Log($"Initializing");
    }

    private void ConfigLoaded(ConfigResponse obj)
    {
        Debug.Log($"Initializing Complete!");
#if !UNITY_EDITOR
        string jsonConfig = ConfigManager.appConfig.GetJson("configuration");
        config = GameConfig.CreateFromJSON(jsonConfig);
#endif
        if(SceneManager.GetActiveScene().buildIndex <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            LoadConfig();
        }
    }
}
