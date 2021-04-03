using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class MapController : MonoBehaviour
{
    public Dictionary<Vector3Int, Tile> Tiles { get; private set; } = new Dictionary<Vector3Int, Tile>();

    public static MapController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Color PulseColor;
    public Color NormalColor;

    private void Start()
    {
    }
    public void InitializeTile(Vector3Int coords, Tile tile)
    {
        Tiles.Add(coords, tile);
    }

    public Tile PickRandomTile()
    {
        List<Tile> AllTiles = new List<Tile>(Tiles.Values);
        return AllTiles[Random.Range(0, AllTiles.Count)];
    }

    public List<Tile> Circle(Tile tile, int range)
    {
        List<Tile> CircleTiles = new List<Tile>();
        foreach (var item in Tiles.Values)
        {
            if(Distance(tile, item) == range)
            {
                CircleTiles.Add(item);
            }
        }
        return CircleTiles;
    }
    public static int Distance(Tile a, Tile b)
    {
        return Distance(a.coords, b.coords);
    }
    public static int Distance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }
    public void Pulse(Tile tile)
    {
        StartCoroutine(PulseCor(tile));
    }
    IEnumerator PulseCor(Tile pulseTile)
    {
        for (int i = 0; i < 10; i++)
        {
            MapController.Instance.Circle(pulseTile, i).ForEach(tile => tile.Ping(1 - i * 0.2f));
            yield return new WaitForSeconds(0.08f);
        }
    }
    private void OnValidate()
    {
        foreach (var item in FindObjectsOfType<Tile>())
        {
            item.GetComponent<SpriteRenderer>().color = NormalColor;
        }
    }
}
