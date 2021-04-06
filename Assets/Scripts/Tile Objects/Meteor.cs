using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Meteor : TileObject
{
    private GameConfig.MeteorConfig config { get => GameManager.Instance.config.meteor; }

    [SerializeField] private Mesh[] Meteors;
    [SerializeField] GameObject BreakParticles;
    [SerializeField] ParticleSystem MiningParticles;

    MeshFilter mf;
    float minerals;

    private void Awake()
    {
        mf = GetComponentInChildren<MeshFilter>();
        Mesh randomMesh = Meteors[Random.Range(0, Meteors.Length)];
        mf.mesh = randomMesh;
        transform.rotation = Quaternion.Euler(0,Random.Range(0,360), 0);
        DestroyImmediate(this.GetComponentInChildren<MeshCollider>());
        var collider = transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
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
    private void FixedUpdate()
    {
        MiningParticles.enableEmission = false;
    }
    public void Mine(float amount)
    {
        MiningParticles.enableEmission = true;

        minerals -= amount;

        if(minerals <= 0)
        {
            Break();
        }
    }
    private void Break()
    {
        Instantiate(BreakParticles, transform.position, Quaternion.identity);
    }
}
