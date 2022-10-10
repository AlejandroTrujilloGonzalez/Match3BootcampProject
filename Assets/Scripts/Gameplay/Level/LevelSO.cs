using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelSO", order = 1)]
public class LevelSO : ScriptableObject
{
    public int id;
    public int moves;
    public int maxTilesTypes;
    public int boardWidth;
    public int boardHeight;
    public EnemySO enemy;
}
