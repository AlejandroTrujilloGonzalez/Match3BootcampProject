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

    [SerializeField]
    private Button _adGoldButton;
    [SerializeField]
    private TMP_Text _goldCostText = null;
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
        _adGoldText.text = _gameConfig.GoldPerAd.ToString();

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