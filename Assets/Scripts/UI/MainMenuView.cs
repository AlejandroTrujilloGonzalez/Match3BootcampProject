using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text energyText;
    [SerializeField]
    private TMP_Text gemsText;
    [SerializeField]
    private TMP_Text goldText;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private LevelListSO levelList;

    private GameProgressionService _gameProgression;

    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    void Start()
    {
        UpdateMainMenuText();
        AudioPlayer.Instance.PlayMusic("Menu1");
    }

    private void UpdateMainMenuText()
    {
        levelText.text = "LEVEL " + (_gameProgression.CurrentLevel + 1);
        energyText.text = _gameProgression.Energy + " / " + (int)GameplayConstants.maxEnergy;
        gemsText.text = _gameProgression.Gems.ToString();
        goldText.text = _gameProgression.Gold.ToString();

        CheckStartButton();
    }

    private void CheckStartButton()
    {
        if (_gameProgression.CurrentLevel >= levelList.levelList.Count)
        {
            levelText.text = "You beat all the levels";
            startButton.interactable = false;
        }

        if (_gameProgression.Energy <= 0)
        {
            levelText.text = "You need more energy!";
            startButton.interactable = false;
        }
    }

    public void ChangeScene(int sceneId)
    {
        SceneLoader.Instance.LoadScene(sceneId);
    }

}
