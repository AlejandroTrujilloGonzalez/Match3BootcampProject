using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel 
{
    public int Id { get; set; }
    public int Moves { get; set; }
    public int MaxTilesTypes; //1 - 5
    public EnemySO Enemy;

    public LevelModel(int id, int moves)
    {
        Id = id;
        Moves = moves;
    }

    public LevelModel(int id, int moves, int maxTilesTypes, EnemySO enemy)
    {
        Id = id;
        Moves = moves;
        MaxTilesTypes = maxTilesTypes;
        Enemy = enemy;
    }
}
