using System.Collections.Generic;
using System.Linq;
using SpykeGames.Showcase.Core;

namespace SpykeGames.Showcase.Core.Dealer
{
    public static class SlotConfig
    {
        private readonly static SlotDeal[] _slotDeals = new SlotDeal[]
        {
                 new SlotDeal(SlotCombinationType.Ace_Wild_Bonus, new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 }),
                 new SlotDeal(SlotCombinationType.Wild_Wild_Seven, new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 }),
                 new SlotDeal(SlotCombinationType.Jackpot_Jackpot_Ace, new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 }),
                 new SlotDeal(SlotCombinationType.Wild_Bonus_Ace, new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 }),
                 new SlotDeal(SlotCombinationType.Bonus_A_Jackpot, new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 }),
                 new SlotDeal(SlotCombinationType.Ace_Ace_Ace, new int[] { 10, 11, 11, 11, 11, 11, 11, 12, 12 }),
                 new SlotDeal(SlotCombinationType.Bonus_Bonus_Bonus, new int[] { 13, 12, 13, 12, 13, 12, 12, 13 }),
                 new SlotDeal(SlotCombinationType.Seven_Seven_Seven, new int[] { 14, 14, 15, 15, 14, 14, 14 }),
                 new SlotDeal(SlotCombinationType.Wild_Wild_Wild, new int[] { 17, 16, 17, 16, 17, 17 }),
                 new SlotDeal(SlotCombinationType.Jackpot_Jackpot_Jackpot, new int[] { 20, 20, 20, 20, 20 }),
        };
       
        public static SlotDeal[] GetAll()
        {
            return _slotDeals;
        }
    }
}
