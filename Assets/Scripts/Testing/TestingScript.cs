using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    private GameProgressionService _gameProgression;

    // Start is called before the first frame update
    void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    public void SetLevel()
    {
        _gameProgression.ResetLevel();
    }

    public void SetEnergy()
    {
        _gameProgression.UpdateEnergy(5);
    }

    public void SetGold()
    {
        _gameProgression.UpdateGold(500);
    }
}
