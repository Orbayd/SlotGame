using System;
using System.Collections.Generic;
using System.Linq;

namespace SpykeGames.Showcase.Core.Dealer
{

    public class SlotDeal
    {
        public SlotCombinationType Id;
        public List<SlotRange> Ranges;
        private int[] _weight;

        public class SlotRange
        {
            public int Min;
            public int Max;
        }
        public SlotDeal(SlotCombinationType id,int[] weight)
        {
            Id = id;
            _weight = weight;
            Ranges = new List<SlotRange>();
            for (int i = 0; i < _weight.Length; i++)
            {
                var range = new SlotRange();
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

        public SlotRange GetRange(int index)
        {
            return Ranges.Find(x => index >= x.Min && index <= x.Max);
        }
    }
}
