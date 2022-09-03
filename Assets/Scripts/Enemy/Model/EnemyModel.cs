using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel 
{
    public string Name { get; set; }
    public int Life { get; set; }
    public string Type { get; set; }

    public EnemyModel(string name, int life)
    {
        Name = name;
        Life = life;
    }
}
