using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace SpykeGames.Showcase.Core.Dealer
{
    public class RepositoryService<T> where T : IModel
    {
        public T Model { get; private set; }

        public RepositoryService(T model)
        {
            Model = model;
        }

        string dataPath = Application.dataPath;
        public void Save()
        {
            File.WriteAllText(Path.Combine(dataPath, Model.GetFilePath()), JsonConvert.SerializeObject(Model));
        }

        public void Load()
        {
            var path  = Path.Combine(dataPath, Model.GetFilePath());
            if(File.Exists(path))
            {
                Model = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
        }
    }

    public class DeckManager
    {
        private RepositoryService<SlotDeck> _repository;
        private SlotDealer _dealer;
        public DeckManager()
        {
            _dealer = new SlotDealer();
            _repository = new RepositoryService<SlotDeck>(new SlotDeck());
            
            Load();
            _dealer.CreateDeck();
            if(_repository.Model.Deck.Count < 20)
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
            
            if(_repository.Model.Deck.Count < 20)
            {
                _dealer.Shuffle();
                _repository.Model.AddCard(_dealer.Hit());
            }
        }
    }
}
