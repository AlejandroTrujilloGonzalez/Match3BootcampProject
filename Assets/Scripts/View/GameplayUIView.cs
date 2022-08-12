using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUIView : MonoBehaviour
{
    public TMP_Text movesLeftText;

    private int movesLeft;

    public static GameplayUIView instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //InitializeLevel(LevelManager.instance.levels[0]);
    }

    public void InitializeLevel(LevelModel level)
    {
        movesLeft = level.maxMoves;
        movesLeftText.text = level.maxMoves.ToString();
    }

    public void DecreaseMoves()
    {
        movesLeft--;
        if (movesLeft <= 0)
        {
            //TODO: LOSE CONDITION
            Debug.Log("you lose");
        }
        movesLeftText.text = movesLeft.ToString();
    }
}