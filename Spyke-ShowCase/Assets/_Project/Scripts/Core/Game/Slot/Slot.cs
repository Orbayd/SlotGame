using SpykeGames.Showcase.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace SpykeGames.Showcase.Core.Column
{
    public class Slot : MonoBehaviour
    {
        [SerializeField]
        public SlotType _cellType;
        private Sprite _normal;
        private Sprite _blurred;
        private Image _imgSlot;
        private bool _isBlurred;
        public bool IsBlurred { get { return _isBlurred; } set{ _isBlurred = value; OnIsBlurredChanged(value);}}


        public void Init(SlotType slotType, Sprite normal, Sprite blurred)
        {
            _imgSlot = GetComponent<Image>();
            _cellType = slotType;
            _normal = normal;
            _blurred = blurred;
            IsBlurred = false;
        }

        private void OnIsBlurredChanged(bool value)
        {
            if(value)
            {
                _imgSlot.sprite = _blurred;
            }
            else
            {
                _imgSlot.sprite = _normal;
            }
        }


    }
}
