using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSprites;
    public List<TileController> tiles = new List<TileController>();
    public Transform[] spawnPoints;
    public GameObject tilePrefab;
    public int rows;
    public float timeBetweenTiles;

    public static TileManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(GenerateTiles());
    }

    internal void MatchTiles(TileController tileController)
    {
        GenerateTileAtLocation(tileController.tile.tileLocation);
        tiles.Remove(tileController);
        GameplayUIController.instance.decreaseMoves();
        Destroy(tileController.gameObject);
    }

    private void GenerateTileAtLocation(Vector2Int tileLocation)
    {
        GameObject tile = Instantiate(tilePrefab, spawnPoints[tileLocation.x]);
        int tileTypeRandomizer = UnityEngine.Random.Range(0, Enum.GetNames(typeof(TileType)).Length);

        Tile tileData = new Tile(tileLocation, (TileType) tileTypeRandomizer);
        tile.GetComponent<TileController>().InitizalizeTile(tileData);
        tiles.Add(tile.GetComponent<TileController>());
    }

    private IEnumerator GenerateTiles()
    {
        for (int i = 0; i < rows; i++)
        {

            for (int j = 0; j < spawnPoints.Length; j++)
            {
                GameObject tile = Instantiate(tilePrefab, spawnPoints[j]);
                Vector2Int location = new Vector2Int(j, i);
                int tileTypeRandomizer = UnityEngine.Random.Range(0, Enum.GetNames(typeof(TileType)).Length);

                Tile tileData = new Tile(location, (TileType) tileTypeRandomizer);
                tile.GetComponent<TileController>().InitizalizeTile(tileData);
                tiles.Add(tile.GetComponent<TileController>());
                yield return new WaitForSeconds(timeBetweenTiles);
            }
        }
        yield return null;
    }
}
