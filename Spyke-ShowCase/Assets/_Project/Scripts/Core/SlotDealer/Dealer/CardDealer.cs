using System;
using System.Linq;
using System.Collections.Generic;
using SpykeGames.Showcase.Core.Enums;

namespace SpykeGames.Showcase.Core.Dealer
{
    public class CardDealer
    {
		private SlotCombinationType[] _array = new SlotCombinationType[100];
		private CardDeal[] _candidates;
		public CardDealer()
        {
			_candidates = DeckConfig.GetAll();
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

		//This is Haram !
		private  Dictionary<CardDeal, int> GetDistanceMap(Dictionary<SlotCombinationType, CardDeal> bound, int index)
		{
			return bound.Select(x => new { Value = x.Value, Distance = x.Value.DistanceToMax(index) }).ToDictionary(x => x.Value, x => x.Distance);
		}

		private void ShiftToLeft(int cIndex, CardDeal match, CardDeal[] candidates)
		{
			List<int> emptyIndex = new List<int>();
			for (int i = cIndex; i > 0; i--)
			{
				if (_array[i] == 0) //If Empty Insert
				{
					emptyIndex.Add(i);
					break;
				}
				var currentRange = candidates.First(x => x.Id == _array[i]).GetRange(i);

				for (int k = i; k >= currentRange.Min; k--)  //Travel as far as range & shift current item to empty
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
			//Travers and Find min Index to set array value
			var IndexRange = match.GetRange(cIndex);
			var minRange = emptyIndex.First();
			for (int j = 0; j < emptyIndex.Count(); j++)
			{
				if (j >= IndexRange.Min && j <= IndexRange.Max)
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
			
				var firstItemRange = _candidates.First(x => x.Id == _array[k]).GetRange(k);
				var SecondItemRange = _candidates.First(x => x.Id == _array[n]).GetRange(n);

				//Two selected item to be shifted has to be InRange
				if ((firstItemRange.Min <= n && firstItemRange.Max >= n) && (SecondItemRange.Min<=k && SecondItemRange.Max >= k)) 
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
