using Game.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class GameProgressionService : IService
{
    [SerializeField]
    private int _gold;
    public int Gold => _gold;

    public int Gems;
    public int Energy;
    public int CurrentLevel;

    public event Action OnInventoryChanged;

    private AnalyticsGameService _analytics;
    private IGameProgressionProvider _progressionProvider;

    public void Initialize(GameConfigService gameConfig, IGameProgressionProvider progressionProvider)
    {
        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _progressionProvider = progressionProvider;
        Load(gameConfig);
    }

    private void Load(GameConfigService config)
    {
        string data = _progressionProvider.Load();
        if (string.IsNullOrEmpty(data))
        {
            Gems = config.InitialGems;
            Energy = config.InitialEnergy;
            _gold = config.InitialGold;
            CurrentLevel = config.InitialLevel;
            Save();
        }
        else
        {
            JsonUtility.FromJsonOverwrite(data, this);
        }
    }

    private void Save()
    {
        _progressionProvider.Save(JsonUtility.ToJson(this));
    }

    public void UpdateGold(int amount)
    {
        _gold += amount;
        OnInventoryChanged?.Invoke();
        Save();
        if (amount > 0)
            _analytics.SendEvent("receive_gold", new Dictionary<string, object>
            {
                ["amount"] = amount
            });
    }

    public void UpdateEnergy(int amount)
    {
        Energy += amount;
        OnInventoryChanged?.Invoke();
        Save();
    }

    public void UpdateGems(int amount)
    {
        Gems += amount;
        OnInventoryChanged?.Invoke();
        Save();
    }

    public void UpdateCurrentLevel(int amount)
    {
        CurrentLevel += amount;
        Save();
    }

    public void SetLevel(int level)
    {
        CurrentLevel = level;
        Save();
    }

    public void SetEnergy(int energy)
    {
        Energy = energy;
        Save();
    }

    public void SetGold(int gold)
    {
        _gold = gold;
        Save();
    }

    //save and load
    //end of save and load

    public void Clear()
    {
    }
}