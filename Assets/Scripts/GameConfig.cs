using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game config", menuName = "Data/Game config"), System.Serializable]
public class GameConfig : ScriptableObject    
{
    [System.Serializable]
    public class PlayerConfig
    {
        public float baseSpeed;
        public float baseDash;
        public float baseRotationSpeed;
        public float baseDrillRange;
    }
    [System.Serializable]
    public class MeteorConfig
    {
        public int baseMaxMeteors;
        public float baseSpawnInterval;
    }

    public PlayerConfig player;
    public MeteorConfig meteor;




    [ContextMenu("Copy config")]
    public void CopyToClickboard()
    {
        TextEditor te = new TextEditor();
        te.text = JsonUtility.ToJson(this);
        te.SelectAll();
        te.Copy();
    }
    public static GameConfig CreateFromJSON(string JSONString)
    {
        GameConfig conf = new GameConfig();
        JsonUtility.FromJsonOverwrite(JSONString, conf);
        return conf;
    }
}
