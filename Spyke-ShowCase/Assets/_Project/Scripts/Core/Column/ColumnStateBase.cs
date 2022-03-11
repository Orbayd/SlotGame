
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public abstract class ColumnStateBase<T> : IColumnState
    {
        protected T Data {get; private set;}
        public ColumnStateBase(T data)
        {
            Data = data;
        }
        public abstract void OnEnter();
        public abstract void OnUpdate(float deltaTime);
        public abstract void OnExit();

        public virtual void SetTarget(Vector3 target)
        {
            
        }
    
    }
}