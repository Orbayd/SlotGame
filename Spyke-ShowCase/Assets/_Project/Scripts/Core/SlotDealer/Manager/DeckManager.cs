using SpykeGames.Showcase.Core.Dealer;
using SpykeGames.Showcase.Core.Enums;
using SpykeGames.Showcase.Core.Repository;

namespace SpykeGames.Showcase.Core.Manager
{
    public class DeckManager
    {
        private Repository<SlotDeck> _repository;
        private CardDealer _dealer;
        public DeckManager()
        {
            _dealer = new CardDealer();
            _repository = new Repository<SlotDeck>(new SlotDeck());

            Load();
            _dealer.CreateDeck();
            if (_repository.Model.Deck.Count < 20)
            {
                _dealer.Shuffle();
                _repository.Model.AddCard(_dealer.Hit());
            }
        }

        public void Save()
        {
            _repository.Save();
        }

        public void Load()
        {
            _repository.Load();
        }

        public SlotCombinationType Peek()
        {
            return _repository.Model.Deck.Peek();
        }

        public void Dequeue()
        {
            _repository.Model.Deck.Dequeue();

            if (_repository.Model.Deck.Count < 20)
            {
                _dealer.Shuffle();
                _repository.Model.AddCard(_dealer.Hit());
            }
        }
    }
}
