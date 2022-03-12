using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public interface IColumnCommand
    {
        void Execute(float elapsedTime);
        void Reset();
        void SetTarget(Vector3 start, Vector3 end);

        bool IsSmooth { get; set; }
        bool AllowLoop { get; set; }
        bool IsFinished { get;}
    }

    public class RollCommand : IColumnCommand
    {
        private Transform _transform;
        private Vector3 _end;
        private Vector3 _current;
        private Vector3 _start;
        private float _duration;
        private float _totalDuration;
        private float _limit;

        public bool IsSmooth { get; set; }
        public bool AllowLoop { get; set; }
        public bool IsFinished => _duration < 0.0;

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

        public void SetTarget(Vector3 start,Vector3 end)
        {
            _end = end;
            _start = start;
            _current = start;
        }
  
        private void Roll(float time)
        {
  
            var t = 1 * ((_totalDuration - time) / _totalDuration);
            //t = _IsSmoothStep ? SmoothStep(t) : t;
            if (IsSmooth)
            {
                _transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(_current, _end, Mathf.SmoothStep(0,1,t));
            }
            else
            {
                _transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(_current, _end,t);
            }

            if (AllowLoop)
            {
                if (_transform.GetComponent<RectTransform>().anchoredPosition3D.y >= _limit)
                {
                    Loop();
                }
            }
            
        }

        //private float SmoothStep(float t)
        //{
        //    return t * t * (3f - 2f * t);
        //}

        private void Loop()
        {
            _duration = _totalDuration;
            _current = _start + _transform.GetComponent<RectTransform>().anchoredPosition3D - _end;
            _transform.GetComponent<RectTransform>().anchoredPosition3D = _current;
        }
    }
}