using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private GameProgressionService _gameProgression;

    private void Awake()
    {
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
    }

    void Start()
    {
        UpdateMainMenuText();
    }

    private void UpdateMainMenuText()
    {
        levelText.text = "LEVEL " + (_gameProgression.CurrentLevel + 1);
        energyText.text = _gameProgression.Energy + " / " + (int)GameplayConstants.maxEnergy;
        gemsText.text = _gameProgression.Gems.ToString();
        goldText.text = _gameProgression.Gold.ToString();
    }

    public void ChangeScene(int sceneId)
    {
        SceneLoader.Instance.LoadScene(sceneId);
    }

}
