using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemieSO", order = 2)]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int life;
    public string type;
    public Sprite sprite;
}
