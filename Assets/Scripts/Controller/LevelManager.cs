using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public List<LevelModel> levels = new List<LevelModel>();

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
        LevelModel level1 = new LevelModel(0, 30);
        levels.Add(level1);
    }
}
