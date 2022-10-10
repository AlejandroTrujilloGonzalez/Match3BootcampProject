public class GameConfigService : IService
{
    public int InitialGold { get; private set; }
    public int InitialGems { get; private set; }
    public int GoldPerWin { get; private set; }
    public int GoldPerDefeat { get; private set; }
    public int GoldPerBooster { get; private set; }
    public int GoldPackCostInGems { get; private set; }
    public int GoldInGoldPack { get; private set; }
    public int GoldPerAd { get; private set; }
    public int GemsPerIAP { get; private set; }

    public void Initialize(RemoteConfigGameService dataProvider)
    {
        InitialGold = dataProvider.Get("InitialGold", 100);
        InitialGems = dataProvider.Get("InitialGems", 10);
        GoldPerWin = dataProvider.Get("GoldPerWin", 10);
        GoldPerDefeat = dataProvider.Get("GoldPerDefeat", 10);
        //GoldPerBooster = dataProvider.Get("GoldPerBooster", 10);
        //GoldPackCostInGems = dataProvider.Get("GoldPackCostInGems", 10);
        GoldInGoldPack = dataProvider.Get("GoldInGoldPack", 10);
        GoldPerAd = dataProvider.Get("GoldPerAd", 10);
        GemsPerIAP = dataProvider.Get("GemsPerIAP", 10);
    }

    public void Clear()
    {
    }
}