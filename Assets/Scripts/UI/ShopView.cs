using System.Collections;
using Game.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    private GameConfigService _gameConfig;
    private GameProgressionService _gameProgression;
    private AdsGameService _adsService;

    //Buttons
    [SerializeField]
    private Button _adGoldButton;
    [SerializeField]
    private Button _buyEnergyButton;

    //Text
    [SerializeField]
    private TMP_Text _goldCostText = null;

    [SerializeField]
    private TMP_Text _energyText = null;
    [SerializeField]
    private TMP_Text _adGoldText = null;

    private void Awake()
    {
        _gameConfig = ServiceLocator.GetService<GameConfigService>();
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        _adsService = ServiceLocator.GetService<AdsGameService>();

    }

    private void Start()
    {
        _goldCostText.text = _gameConfig.EnergyGoldCost.ToString() + " Gold";
        _energyText.text = _gameConfig.EnergyPerBuy.ToString() + " Energy";
        _adGoldText.text = _gameConfig.GoldPerAd.ToString() + " Gold";

        UpdateCards();
    }

    private void UpdateCards()
    {
        _adGoldButton.interactable = true;
        StopAllCoroutines();
        if (!_adsService.IsAdReady)
        {
            _adGoldButton.interactable = false;
            StartCoroutine(WaitForAdReady());
        }
    }

    IEnumerator WaitForAdReady()
    {
        while (!_adsService.IsAdReady)
        {
            yield return new WaitForSeconds(0.5f);
        }

        _adGoldButton.interactable = true;
    }

    //Buttons behaviour

    public void PurchaseEnergy()
    {
        _gameProgression.UpdateEnergy(_gameConfig.EnergyPerBuy);
        _gameProgression.UpdateGold(-_gameConfig.EnergyGoldCost);
        UpdateCards();
    }

    public async void PurchaseAdGold()
    {
        if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _gameProgression.UpdateGold(_gameConfig.GoldPerAd);
            UpdateCards();
        }
        else
        {
            Debug.LogError("ad failed");
        }
    }
}