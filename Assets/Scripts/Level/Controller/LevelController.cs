using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    private LevelModel model;

    public LevelController(int id, int moves)
    {
        model = new LevelModel(id, moves);
    }

    public int GetId()
    {
        return model.Id;
    }

    public int GetMoves()
    {
        return model.Moves;
    }

    public void DecreaseMoves()
    {
        model.Moves--;
    }

    public void GameOver()
    {
        SceneLoader.Instance.LoadScene(0);
    }

}
