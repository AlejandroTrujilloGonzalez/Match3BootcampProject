using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelList", order = 3)]
public class LevelListSO : ScriptableObject
{
    public List<LevelSO> levelList;
}
