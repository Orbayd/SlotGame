using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace SpykeGames.Showcase.Core.Repository
{
    public class Repository<T> where T : IModel
    {
        public T Model { get; private set; }

        public Repository(T model)
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
}
