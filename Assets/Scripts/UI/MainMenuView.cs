using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuView : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text energyText;
    public TMP_Text gemsText;
    public TMP_Text goldText;

    public int maxEnergy = 5;

    void Start()
    {
        UpdateAllText();
    }

    private void UpdateAllText()
    {
        levelText.text = "LEVEL " + (DataController.Instance.data.playerCurrentLevel + 1).ToString();
        energyText.text = DataController.Instance.data.playerEnergy.ToString() + " / " + maxEnergy;
        gemsText.text = DataController.Instance.data.playerGems.ToString();
        goldText.text = DataController.Instance.data.playerGold.ToString();
    }

    public void ChangeScene(int sceneId)
    {
        SceneLoader.Instance.LoadScene(sceneId);
    }

}
