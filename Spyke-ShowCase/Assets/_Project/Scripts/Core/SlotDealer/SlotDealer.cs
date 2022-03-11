using System;
using System.Linq;
using System.Collections.Generic;

namespace SpykeGames.Showcase.Core.Dealer
{
    public class SlotDealer
    {
		private SlotCombinationType[] _array = new SlotCombinationType[100];
		private SlotDeal[] _candidates;
		public SlotDealer()
        {
			_candidates = SlotConfig.GetAll();
		}
		public SlotCombinationType[] CreateDeck()
		{
			var boundMap = _candidates.ToDictionary(x => x.Id, x => x);

			for (int i = 0; i < _array.Length; i++)
			{
				var distanceMap = GetDistanceMap(boundMap, i);
				foreach (var candidate in distanceMap)
				{
					if (candidate.Value == 0)
					{
						if (_array[i] == 0)
						{
							_array[i] = candidate.Key.Id;
						}
						else
						{
							ShiftToLeft(i, candidate.Key, _candidates);
						}
					}
				}
			}
			return _array;
		}
		private  Dictionary<SlotDeal, int> GetDistanceMap(Dictionary<SlotCombinationType, SlotDeal> bound, int index)
		{
			return bound.Select(x => new { Value = x.Value, Distance = x.Value.DistanceToMax(index) }).ToDictionary(x => x.Value, x => x.Distance);
		}
		private void ShiftToLeft(int cIndex, SlotDeal match, SlotDeal[] candidates)
		{
			List<int> emptyIndex = new List<int>();
			for (int i = cIndex; i > 0; i--)
			{
				if (_array[i] == 0)
				{
					emptyIndex.Add(i);
					break;
				}
				var range = candidates.First(x => x.Id == _array[i]).GetRange(i);

				for (int k = i; k >= range.Min; k--)
				{
					if (_array[k] == 0)
					{
						_array[k] = _array[i];
						_array[i] = 0;

						emptyIndex.Add(i);
						continue;
					}

				}
			}

			var r = match.GetRange(cIndex);
			var minRange = emptyIndex.First();
			for (int j = 0; j < emptyIndex.Count(); j++)
			{
				if (j >= r.Min && j <= r.Max)
				{
					if (minRange > emptyIndex[j])
					{
						minRange = emptyIndex[j];
					}
				}
			}
			_array[minRange] = match.Id;

		}
		public SlotCombinationType[] Shuffle()
		{
			var randLenght = 2;
			int n = _array.Length - 1;
			while (n > 1)
			{

				int k = UnityEngine.Random.Range(0,5) - randLenght + n;
				if (k >= _array.Length || k < 1)
				{
					continue;
				}
				var krange = _candidates.First(x => x.Id == _array[k]).GetRange(k);
				var nrange = _candidates.First(x => x.Id == _array[n]).GetRange(n);


				if ((krange.Min <= n && krange.Max >= n) && (nrange.Min<=k && nrange.Max >= k))
				{
					var temp = _array[n];
					_array[n] = _array[k];
					_array[k] = temp;
				}
				n--;
			}
			return _array;
		}

		public SlotCombinationType[] Hit()
		{
			return _array;
		}
	}
}
