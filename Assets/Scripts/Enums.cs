namespace MythicEmpire.Enums
{
    public enum EventID
    {
        None,
        OnLoginSuccess,
        OnLoginFail,
        OnRegisterSuccess,
        OnRegisterFail,
        SelectedCard,
        OnUpdateNicknameSuccess,
        OnUpgradeCardSuccess,
        OnBuyCardSuccess,
        OnBuyGachaSuccess,
        PrepareListCard,
        OnStartGame,
        OnEndGame,
        ServeReceiveMatchMaking,
        CancelMatchMakingSuccess,
        PlaceCard
    }
    public enum GachaType
    {
        None,
        Common,
        Rare,
        Legend
    }
    public enum ModeGame
    {
        None,
        Adventure,
        Arena

    }
    public enum GameState
    {
        None,
        Init,
        Start,
        Playing,
        EndGame
    }
    public enum CardType
    {
        None,
        TowerCard,
        MonsterCard,
        SpellCard
    }
    public enum RarityCard
    {
        None,
        Common,
        Rare,
        Mythic,
        Legend

    }
    public enum TypeTile
    {
        None,
        Player,
        Opponent
    }
    public enum TypePlayer
    {
        Player,
        Opponent
    }
}