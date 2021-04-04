using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), SelectionBase]
public class Tile : MonoBehaviour
{
    const float hexH = 6.928203f;
    const float hexW = 6f;

    public TileObject tileObject { get; private set; }

    private SpriteRenderer sr;
    public Vector3Int coords { get; private set; }
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        int z = Mathf.RoundToInt(transform.position.z / hexW);
        int x = Mathf.RoundToInt((transform.position.x-(z*(hexH/2))) / hexH);
        int y = -(x + z);

        coords = new Vector3Int(x,y,z);

        MapController.Instance.InitializeTile(coords, this);
    }
    public void Ping(float alpha = 1)
    {
        Color pingColor = MapController.Instance.PulseColor;
        sr.DOColor(pingColor, 0.1f).OnComplete(() => sr.DOColor(MapController.Instance.NormalColor, 0.1f));
        transform.DOLocalMoveY(0.3f, 0.4f).OnComplete(() => transform.DOLocalMoveY(0, 0.4f));
    }
    public void SetObject(TileObject tileObject)
    {
        tileObject.tile = this;
        this.tileObject = tileObject;
    }
    public void RemoveObject()
    {
        tileObject = null;
    }
}
