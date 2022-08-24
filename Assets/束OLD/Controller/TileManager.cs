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

    public void OnClickTile(TileController tileController)
    {

        List<TileController> neighbours = new List<TileController>();
        List<TileController> furtherNeighbours = new List<TileController>();
        foreach (TileController tile in tiles)
        {
            if (tile.validNeighbours.Contains(tileController))
            {
                neighbours.Add(tile);
            }
            else
            {
                furtherNeighbours.Add(tile);
            }
        }

        while (furtherNeighbours.Count > 0)
        {
            TileController checkingTile = furtherNeighbours[0];
            furtherNeighbours.Remove(checkingTile);
            bool addNeighbours = false;
            foreach (TileController tile in neighbours)
            {
                if (checkingTile.validNeighbours.Contains(tile))
                {
                    addNeighbours = true;
                    break;
                }

            }
            if (addNeighbours) neighbours.Add(checkingTile);
        }
        StartCoroutine(CheckMatch(tileController, neighbours));
    }

    private IEnumerator CheckMatch(TileController tileController, List<TileController> directNeighbors)
    {
        GenerateTileAtLocation(tileController.tile.tileLocation);
        tiles.Remove(tileController);
        Destroy(tileController.gameObject);
        foreach (TileController tile in directNeighbors)
        {
            GenerateTileAtLocation(tile.tile.tileLocation);
            tiles.Remove(tile);
            if (tile != null) Destroy(tile.gameObject);
        }
        GameplayUIView.instance.DecreaseMoves();
        yield return null;
    }

    private IEnumerator ReassignLocation()
    {
        yield return new WaitForEndOfFrame();
        foreach (TileController tile in tiles)
        {
            int parentIndex = 0;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i] == tile.transform.parent)
                {
                    parentIndex = i;
                    break;
                }
            }
            tile.tile.tileLocation = new Vector2Int(parentIndex, tile.transform.GetSiblingIndex());
        }
    }

    private void GenerateTileAtLocation(Vector2Int tileLocation)
    {
        GameObject tile = Instantiate(tilePrefab, spawnPoints[tileLocation.x]);
        int tileTypeRandomizer = UnityEngine.Random.Range(0, Enum.GetNames(typeof(TileType)).Length);

        Tile tileData = new Tile(tileLocation, (TileType) tileTypeRandomizer);
        tile.GetComponent<TileController>().InitizalizeTile(tileData);
        tiles.Add(tile.GetComponent<TileController>());
        StartCoroutine(ReassignLocation());
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

    public List<TileController> GetTileNeighbors(TileController tile)
    {
        List<TileController> neighbors = new List<TileController>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                if (Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1) continue;
                int checkX = tile.tile.tileLocation.x + j;
                int checkY = tile.tile.tileLocation.y + i;
                if (checkY <= 5 && checkX >= 0 && checkY >= 0 && checkX <= 4 &&
                    FindTileByLocation(new Vector2Int(checkX, checkY)).tile.type == tile.tile.type
    )
                {
                    neighbors.Add(FindTileByLocation(new Vector2Int(checkX, checkY)));
                }
            }
        }
        return neighbors;
    }

    private TileController FindTileByLocation(Vector2Int location)
    {
        foreach (TileController tile in tiles)
        {
            if (tile.tile.tileLocation == location)
            {
                return tile;
            }
        }
        return null;
    }
}
