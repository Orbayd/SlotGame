using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class Cell : MonoBehaviour
    {
        public SlotType CellType;
        private Sprite _normal;
        private Sprite _blurred;
        private bool _isBlurred;

        private SpriteRenderer _renderer;

        public bool IsBlurred { get { return _isBlurred; } set{ _isBlurred = value; OnIsBlurredChanged(value);}}


        public void Init(SlotType slotType, Sprite normal, Sprite blurred)
        {
            CellType = slotType;
            _normal = normal;
            _blurred = blurred;
            _renderer = GetComponent<SpriteRenderer>();
            IsBlurred = false;
        }

        private void OnIsBlurredChanged(bool value)
        {
            //Debug.Log($"[Info] State Changed{ value }");
            if(value)
            {
                _renderer.sprite = _blurred;
            }
            else
            {
                _renderer.sprite = _normal;
            }
        }


    }
}