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
        ServerReceiveMatchMaking,
        CancelMatchMakingSuccess,
        ResetPasswordSuccess,
        ConfirmOTPSuccess,
        SendOTPSuccess,
        DeselectCardPrepare,
        UpdateEnergy,
        UpdateWaveTime,
        RenderListCard,
        OnGetMap,
        OnGetCard,
        BuildTower,
        CreateMonster,
        PlaceSpell
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
    public enum OwnerType
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
    public enum GameResult
    {
        Win,
        Draw,
        Loss
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}