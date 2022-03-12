
using SpykeGames.Showcase.Core.Enums;
using UnityEngine;

namespace SpykeGames.Showcase.Core.Column
{
    public class RollState : ColumnStateBase<SlotColumn>
    {
        public ColumnStateType State => ColumnStateType.Rolling;

        private ColumnConfig _config;
        private Vector2 _endPosition;
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private IColumnCommand _command;
        private float _spinTime;
        private float _initialSpinTime;

        private RectTransform _rectTransform;

        public RollState(SlotColumn data, ColumnConfig config ,IColumnCommand command) : base(data)
        {
            _rectTransform = Data.GetComponent<RectTransform>();

            _config = config;
            _command = command;
            _initialSpinTime =  Data.ColumnId == 0 ? config.MaxSpintTime : float.MaxValue;
            
            var temp = _rectTransform.anchoredPosition;
            temp.y = _config.MaxLimit;
            _endPosition = temp;

            temp = _rectTransform.anchoredPosition;
            temp.y = _config.MinLimit;
            _startPosition = temp;

        }

        public override void OnEnter()
        {
            _spinTime = _initialSpinTime;
            _command.IsSmooth = false;
            _command.AllowLoop = true;
            _command.SetTarget(_startPosition,_endPosition);
            SetAllSlotsBlurred(true);
        }

        public override void OnUpdate(float deltaTime)
        {
            _spinTime-=deltaTime;
            if (_spinTime > 0)
            {
                _command.Execute(Time.deltaTime);
            }
            else if (_startPosition.y > _rectTransform.anchoredPosition.y && Vector2.Distance(_rectTransform.anchoredPosition, _startPosition) > 30f) //Last Roll
            {
                _command.Execute(Time.deltaTime);
            }
            else
                Data.ChangeState(ColumnStateType.Stoping);
        }

        public override void OnExit()
        {
            _command.SetTarget(_startPosition,_targetPosition);
            _command.Reset();
            SetAllSlotsBlurred(false);
        }

        public override void SetTarget(Vector2 target)
        {
            _targetPosition = target;
        }

        private void SetAllSlotsBlurred(bool value)
        {
            var cells = Data.GetComponentsInChildren<Slot>();
            foreach (var slot in cells)
            {
                slot.IsBlurred = value;
            }
        }
    }
}