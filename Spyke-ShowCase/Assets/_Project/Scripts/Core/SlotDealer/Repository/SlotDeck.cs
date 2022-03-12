using System;
using System.Collections.Generic;
using System.Linq;
using SpykeGames.Showcase.Core.Enums;

namespace SpykeGames.Showcase.Core.Repository
{
    public interface IModel
    {
        string GetFilePath();   
    }
    
    [Serializable]
    public class SlotDeck : IModel
	{
		public Queue<SlotCombinationType> Deck {get;set;} = new Queue<SlotCombinationType>();

        public string GetFilePath()
        {
            return "Deck.json";
        }

        public void AddCard(IEnumerable<SlotCombinationType> combinations)
        {
            var newList = Deck.ToList();
            newList.AddRange(combinations);
            Deck = new Queue<SlotCombinationType>(newList);
        }
    }
}
