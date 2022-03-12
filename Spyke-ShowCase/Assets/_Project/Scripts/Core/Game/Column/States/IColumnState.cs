using UnityEngine;

namespace SpykeGames.Showcase.Core.Column
{
    public interface IColumnState
    {
        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
        void SetTarget(Vector2 target);
    }
}