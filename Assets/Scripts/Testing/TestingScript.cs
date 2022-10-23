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
        _gameProgression.SetLevel(0);
    }

    public void SetEnergy()
    {
        _gameProgression.SetEnergy(0);
    }

    public void SetGold()
    {
        _gameProgression.SetGold(500);
    }
}
