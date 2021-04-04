using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : TileObject
{
    private GameConfig.MeteorConfig config { get => GameManager.Instance.config.meteor; }

    [SerializeField] private Mesh[] Meteors;

    MeshFilter mf;
    private void Awake()
    {
        mf = GetComponentInChildren<MeshFilter>();
        Mesh randomMesh = Meteors[Random.Range(0, Meteors.Length)];
        mf.mesh = randomMesh;
        transform.rotation = Quaternion.Euler(0,Random.Range(0,360), 0);
        DestroyImmediate(this.GetComponent<MeshCollider>());
        var collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = randomMesh;
    }
    private void OnDestroy()
    {
        tile.RemoveObject();
        MeteorController.Instance.MeteorDestroyed();
    }
    public void Land()
    {
        MapController.Instance.Pulse(tile);
    }

    public void Drill()
    {

    }
}
