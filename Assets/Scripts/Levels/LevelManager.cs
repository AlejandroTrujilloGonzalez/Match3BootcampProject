using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public List<Level> levels = new List<Level>();

    public static LevelManager instance;

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
        GenerateLevels();
    }

    private void GenerateLevels()
    {
        Level level1 = new Level(0, 30);
        levels.Add(level1);
    }
}
