using System;
using System.Collections.Generic;
using System.Linq;
using SpykeGames.Showcase.Core.Enums;

namespace SpykeGames.Showcase.Core.Dealer
{

    public class CardDeal
    {
        public SlotCombinationType Id;
        public List<CardRange> Ranges;
        private int[] _weight;

        public class CardRange
        {
            public int Min;
            public int Max;
        }
        public CardDeal(SlotCombinationType id,int[] weight)
        {
            Id = id;
            _weight = weight;
            Ranges = new List<CardRange>();
            for (int i = 0; i < _weight.Length; i++)
            {
                var range = new CardRange();
                range.Max = CalculateRange(i + 1) - 1;
                range.Min = CalculateRange(i);
                Ranges.Add(range);
            }   
        }
        private int CalculateRange(int i)
        {
            return _weight.Take(i).Sum();
        }

        public int DistanceToMax(int index)
        {
            var period = Ranges.Find(x => index >= x.Min && index <= x.Max);
            return period.Max - index;
        }

        public CardRange GetRange(int index)
        {
            return Ranges.Find(x => index >= x.Min && index <= x.Max);
        }
    }
}
