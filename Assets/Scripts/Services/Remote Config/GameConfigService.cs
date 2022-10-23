public class GameConfigService : IService
{
    public int InitialGold { get; private set; }
    public int InitialGems { get; private set; }
    public int InitialEnergy { get; private set; }
    public int InitialLevel { get; private set; }
    public int GoldPerWin { get; private set; }
    public int GoldPerDefeat { get; private set; }
    public int GoldPerBooster { get; private set; }
    public int GoldPackCostInGems { get; private set; }
    public int GoldInGoldPack { get; private set; }
    public int GoldPerAd { get; private set; }
    public int GemsPerIAP { get; private set; }
    public int EnergyGoldCost { get; private set; }
    public int EnergyPerBuy { get; private set; }
    
    public void Initialize(RemoteConfigGameService dataProvider)
    {
        InitialGold = dataProvider.Get("InitialGold", 100);
        InitialGems = dataProvider.Get("InitialGems", 10);
        InitialEnergy = dataProvider.Get("InitialEnergy", 5);
        InitialLevel = dataProvider.Get("InitialLevel", 0);
        GoldPerWin = dataProvider.Get("GoldPerWin", 10);
        GoldPerDefeat = dataProvider.Get("GoldPerDefeat", 10);
        GoldPerAd = dataProvider.Get("GoldPerAd", 10);
        EnergyGoldCost = dataProvider.Get("EnergyGoldCost", 100);
        EnergyPerBuy = dataProvider.Get("EnergyPerBuy", 1);
    }

    public void Clear()
    {
    }
}