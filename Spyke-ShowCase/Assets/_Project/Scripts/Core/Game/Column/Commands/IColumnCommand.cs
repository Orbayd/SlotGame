using UnityEngine;

namespace SpykeGames.Showcase.Core.Column
{
    public interface IColumnCommand
    {
        void Execute(float elapsedTime);
        void Reset();
        void SetTarget(Vector2 start, Vector2 end);
        void SetTime(float t);

        bool IsSmooth { get; set; }
        bool AllowLoop { get; set; }
        bool IsFinished { get;}
    }
}