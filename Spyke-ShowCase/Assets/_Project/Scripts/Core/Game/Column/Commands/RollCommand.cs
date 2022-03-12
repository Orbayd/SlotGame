using UnityEngine;

namespace SpykeGames.Showcase.Core.Column
{
    public class RollCommand : IColumnCommand
    {
        private RectTransform _transform;
        private Vector2 _end;
        private Vector2 _current;
        private Vector2 _start;
        private float _duration;
        private float _totalDuration;
        private float _limit;

        public bool IsSmooth { get; set; }
        public bool AllowLoop { get; set; }
        public bool IsFinished => _duration < 0.0;

        public RollCommand(RectTransform transform, float duration, float limit)
        {
          
            SetTime(duration);
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

        public void SetTarget(Vector2 start, Vector2 end)
        {
            _end = end;
            _start = start;
            _current = start;
        }

        public void SetTime(float time)
        {
            _duration = time;
            _totalDuration = time;
        }
        //Should use Tweener
        private void Roll(float time)
        {

            var t = 1 * ((_totalDuration - time) / _totalDuration);
            if (IsSmooth)
            {
                
                //t = SmoothStep(t);
                _transform.anchoredPosition = Vector3.Lerp(_current, _end,Mathf.SmoothStep(0f,1f,t));
                //Debug.Log($"[INFO] Smooth Lerp Time :{t}");
                //_transform.anchoredPosition = Vector3.Lerp(_transform.anchoredPosition, _end, Time.deltaTime);
            }
            else
            {
                _transform.anchoredPosition = Vector3.Lerp(_current, _end, t);
            }

            if (AllowLoop)
            {
                if (_transform.anchoredPosition3D.y >= _limit)
                {
                    Loop();
                }
            }

        }

        private float SmoothStep(float t)
        {
            return t * t * (3f - 2f * t);
        }

        private void Loop()
        {
            _duration = _totalDuration;
            _current = _start + _transform.anchoredPosition - _end;
            _transform.anchoredPosition3D = _current;
        }
    }
}