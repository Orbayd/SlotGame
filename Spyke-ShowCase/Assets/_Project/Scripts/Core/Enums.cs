
namespace SpykeGames.Showcase.Core.Enums
{
    public enum SlotCombinationType
    {
        None,
        Ace_Wild_Bonus,
        Wild_Wild_Seven,
        Jackpot_Jackpot_Ace,
        Wild_Bonus_Ace,
        Bonus_A_Jackpot,
        Ace_Ace_Ace,
        Bonus_Bonus_Bonus,
        Seven_Seven_Seven,
        Wild_Wild_Wild,
        Jackpot_Jackpot_Jackpot
    }

    public enum SlotType
    {
        Ace, Jackpot, Wild, Bonus, Seven
    }

    public enum ColumnStateType
    {
        Ready, Rolling, Stoping
    }

    public enum StopDelayType
    {
        Immediate, Normal, Slow
    }
}
