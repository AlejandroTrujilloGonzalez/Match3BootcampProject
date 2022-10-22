using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevelScript : MonoBehaviour
{
    private GameProgressionService _gameProgression;

    // Start is called before the first frame update
    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    public void ResetLevel()
    {
        _gameProgression.CurrentLevel = 0;
    }
}
