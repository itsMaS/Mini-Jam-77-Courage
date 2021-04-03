using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.RemoteConfig;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public static MeteorController Instance;

    [SerializeField] GameObject[] Meteors;

    private GameConfig.MeteorConfig config { get => GameManager.Instance.config.meteor; }

    int meteorCount;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(SpawnMeteors());
    }

    IEnumerator SpawnMeteors()
    {
        while(true)
        {
            yield return new WaitForSeconds(config.baseSpawnInterval);
            if(meteorCount < config.baseMaxMeteors)
            {
                meteorCount++;
                Tile tile = SelectMeteorTile();
                Meteor meteor = Instantiate(Meteors[Random.Range(0, Meteors.Length)], tile.transform.position, Quaternion.identity, transform).GetComponent<Meteor>();
                tile.SetObject(meteor);
            }
        }
    }
    public Tile SelectMeteorTile()
    {
        List<Tile> Tiles = new List<Tile>(MapController.Instance.Tiles.Values);
        Tile[] Selection = Tiles.Where(tile => !tile.tileObject).ToArray();
        return Selection[Random.Range(0, Selection.Length)];
    }
    public void MeteorDestroyed()
    {
        meteorCount--;
    }
}
