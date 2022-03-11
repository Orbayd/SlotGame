using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpykeGames.Showcase.Core.Dealer
{
    public class SlotCombination
    {
        public ReadOnlyCollection<SlotType> Slots { get; private set; }
        public StopDelayType StopDelay { get; private set; }
        public int Reward { get; private set; }
        private SlotCombination() { }
        public static SlotCombination GetOrCreate(SlotCombinationType type)
        {
            return SlotCombinationFactory.Create(type);
        }
        private static class SlotCombinationFactory
        {
            private readonly static Dictionary<SlotCombinationType, SlotCombination> _slotCombinationMap = new Dictionary<SlotCombinationType,SlotCombination>();
            public static SlotCombination Create(SlotCombinationType type)
            {
                if(_slotCombinationMap.ContainsKey(type))
                {
                    return _slotCombinationMap[type];
                }
                var slot = new SlotCombination();
                switch (type)
                {
                    case SlotCombinationType.Ace_Wild_Bonus:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Ace, SlotType.Wild, SlotType.Bonus });
                        slot.Reward = 0;
                        slot.StopDelay = StopDelayType.Immediate;
                        break;
                    case SlotCombinationType.Wild_Wild_Seven:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Wild, SlotType.Wild, SlotType.Seven });
                        slot.Reward = 0;
                        slot.StopDelay = StopDelayType.Normal;
                        break;
                    case SlotCombinationType.Jackpot_Jackpot_Ace:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Jackpot, SlotType.Jackpot, SlotType.Ace });
                        slot.Reward = 0;
                        slot.StopDelay = StopDelayType.Normal;
                        break;
                    case SlotCombinationType.Wild_Bonus_Ace:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Wild, SlotType.Bonus, SlotType.Ace });
                        slot.Reward = 0;
                        slot.StopDelay = StopDelayType.Immediate;
                        break;
                    case SlotCombinationType.Bonus_A_Jackpot:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Bonus, SlotType.Ace, SlotType.Jackpot });
                        slot.Reward = 0;
                        slot.StopDelay = StopDelayType.Immediate;
                        break;
                    case SlotCombinationType.Ace_Ace_Ace:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Ace, SlotType.Ace, SlotType.Ace });
                        slot.Reward = 20;
                        slot.StopDelay = StopDelayType.Slow;
                        break;
                    case SlotCombinationType.Bonus_Bonus_Bonus:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Bonus, SlotType.Bonus, SlotType.Bonus });
                        slot.Reward = 30;
                        slot.StopDelay = StopDelayType.Slow;
                        break;
                    case SlotCombinationType.Seven_Seven_Seven:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Seven, SlotType.Seven, SlotType.Seven });
                        slot.Reward = 40;
                        slot.StopDelay = StopDelayType.Slow;
                        break;
                    case SlotCombinationType.Wild_Wild_Wild:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Wild, SlotType.Wild, SlotType.Wild });
                        slot.Reward = 50;
                        slot.StopDelay = StopDelayType.Slow;
                        break;
                    case SlotCombinationType.Jackpot_Jackpot_Jackpot:
                        slot.Slots = Array.AsReadOnly(new SlotType[] { SlotType.Jackpot, SlotType.Jackpot, SlotType.Jackpot });
                        slot.Reward = 100;
                        slot.StopDelay = StopDelayType.Slow;
                        break;
                    default: break;
                }
                _slotCombinationMap.Add(type,slot);
                return slot;
            }
        }
    }
}


