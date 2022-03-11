using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public interface IColumnState
    {
        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
        void SetTarget(Vector3 target);
    }
}