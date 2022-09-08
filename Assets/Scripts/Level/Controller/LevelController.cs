using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    private LevelModel model;

    public LevelController(int id, int moves, int maxTilesTypes)
    {
        model = new LevelModel(id, moves, maxTilesTypes);
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
        Debug.Log("You win");
        SceneLoader.Instance.LoadScene(0);
    }

    public void GameOver()
    {
        DataController.Instance.data.playerGold = DataController.Instance.data.playerGold + (int)GameplayConstants.goldPerDefeat;
        DataManager.Save();
        SceneLoader.Instance.LoadScene(0);
    }

}
