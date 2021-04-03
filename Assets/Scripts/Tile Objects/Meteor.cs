using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : TileObject
{
    [SerializeField] private Mesh[] States = new Mesh[3];

    MeshRenderer mr;
    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
    }
    private void OnDestroy()
    {
        MeteorController.Instance.MeteorDestroyed();
    }
    public void Land()
    {
        MapController.Instance.Pulse(tile);
    }
}
