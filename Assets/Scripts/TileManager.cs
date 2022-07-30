using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSprites;
    public List<TileController> tileControllers = new List<TileController>();
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

    private IEnumerator GenerateTiles()
    {
        for (int i = 0; i < rows; i++)
        {

            for (int j = 0; j < spawnPoints.Length; j++)
            {
                GameObject tile = Instantiate(tilePrefab, spawnPoints[j]);
                yield return new WaitForSeconds(timeBetweenTiles);
            }
        }
        yield return null;
    }
}
