using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class LoadingController
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

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
        Debug.Log($"Initializing");
    }
}
