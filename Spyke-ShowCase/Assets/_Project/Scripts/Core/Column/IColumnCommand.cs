using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public interface IColumnCommand
    {
        void Execute(float elapsedTime);
        void Reset();
        void SetTarget(Vector3 start, Vector3 end);
        bool IsFinished();
    }

    public class RollCommand : IColumnCommand
    {
        private Vector3 _end;
        private Vector3 _current;
        private Vector3 _start;
        private float _duration;
        private float _totalDuration;
        private float _limit;
        private Transform _transform;
        private bool _IsSmoothStep;

        public RollCommand(Transform transform, float duration , float limit)
        {
            _duration = duration;
            _totalDuration = duration;
            _limit = limit;
            _transform = transform;
        }
        public void Execute(float elapsedTime)
        {
            _duration -= elapsedTime;
            Roll(_duration);
        }

        public void Reset()
        {
            _duration = _totalDuration;   
        }
        public bool IsFinished()
        {
            return _duration < 0.0f;
        }
        public void SetTarget(Vector3 start,Vector3 end)
        {
            _end = end;
            _start = start;
            _current = start;
        }

        public void SetSmootStep(bool value)
        {
            _IsSmoothStep = value;
        }

        private void Roll(float time)
        {
            if (_transform.position.y >= _limit)
            {
                Loop();
            }   
            var t = 1 * ((_totalDuration - time) / _totalDuration);
            t = _IsSmoothStep ? SmoothStep(t) : t; 
            _transform.position = Vector3.Lerp(_current, _end,t);
        }

        private float SmoothStep(float t)
        {
            return t * t * (3f - 2f * t);
        }

        private void Loop()
        {
            _duration = _totalDuration;
            // var temp = _transform.localPosition;
            // temp -= _end;
            // _current = _start + temp;
            _current = _start + _transform.position - _end;
            // _transform.localPosition = _current;
            _transform.position = _current;
        }
    }
}