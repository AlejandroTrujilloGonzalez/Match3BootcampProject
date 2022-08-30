using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemieSO", order = 2)]
public class EnemySO : ScriptableObject
{
    public string enemieName;
    public int life;
    public SpriteRenderer sprite;
}
