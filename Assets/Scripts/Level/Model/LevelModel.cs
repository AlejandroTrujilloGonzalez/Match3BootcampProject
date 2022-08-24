using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel 
{
    public int Id { get; set; }
    public int Moves { get; set; }
    public int maxTilesTypes; //1 - 5

    public LevelModel(int id, int moves)
    {
        Id = id;
        Moves = moves;
    }
}
