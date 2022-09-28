using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    private LevelModel model;

    public LevelController(int id, int moves, int maxTilesTypes, EnemySO enemy)
    {
        model = new LevelModel(id, moves, maxTilesTypes, enemy);
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

    public void WinLevel()
    {
        DataController.Instance.data.playerCurrentLevel++;
        DataController.Instance.data.playerGold = DataController.Instance.data.playerGold + (int)GameplayConstants.goldPerVictory;
        DataManager.Save();
    }

    public void LoseLevel()
    {
        DataController.Instance.data.playerGold = DataController.Instance.data.playerGold + (int)GameplayConstants.goldPerDefeat;
        DataController.Instance.data.playerEnergy--;
        DataManager.Save();
    }

}
