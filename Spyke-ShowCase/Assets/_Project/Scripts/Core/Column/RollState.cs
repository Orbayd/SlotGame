
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class RollState : ColumnStateBase<ColumnView>
    {
        public ColumnStateType State => ColumnStateType.Rolling;
        private ColumnConfig _config;
        private Vector3 _endPosition;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _spinTime;
        private float _initialSpinTime;
        private IColumnCommand _command;

        public RollState(ColumnView data, ColumnConfig config ,IColumnCommand command) : base(data)
        {
            _config = config;
            _command = command;
            _initialSpinTime =  Data.ColumnId == 0 ? config.MaxSpintTime : float.MaxValue;
            
            var temp = Data.GetComponent<RectTransform>().anchoredPosition3D;
            temp.y = _config.MaxLimit;
            _endPosition = temp;

            temp = Data.GetComponent<RectTransform>().anchoredPosition3D;
            temp.y = _config.MinLimit;
            _startPosition = temp;

        }

        public override void OnEnter()
        {
            _spinTime = _initialSpinTime;
            _command.SetTarget(_startPosition,_endPosition);
            _command.IsSmooth = false;
            _command.AllowLoop = true;
            SetAllSlotsBlurred(true);
        }

        public override void OnUpdate(float deltaTime)
        {
            _spinTime-=deltaTime;
            if(_spinTime > 0)
            {
                _command.Execute(Time.deltaTime);
            }
            else if(Vector3.Distance(Data.transform.GetComponent<RectTransform>().anchoredPosition3D, _endPosition)> 30) //Last Roll
            {
                _command.Execute(Time.deltaTime);
                //Debug.Log($"{Data.ColumnId} Last Roll: Distance{Vector3.Distance(Data.transform.position, _targetPosition)}");
            }
            else
                Data.ChangeState(ColumnStateType.Stoping);
        }

        public override void OnExit()
        {
            Data.transform.GetComponent<RectTransform>().anchoredPosition3D = _targetPosition;
            _command.SetTarget(Data.GetComponent<RectTransform>().anchoredPosition3D,_targetPosition);
            SetAllSlotsBlurred(false);
            _command.Reset();
        }

        public override void SetTarget(Vector3 target)
        {
            _targetPosition = target;
        }

        private void SetAllSlotsBlurred(bool value)
        {
            var cells = Data.GetComponentsInChildren<SlotView>();
            foreach (var slot in cells)
            {
                slot.IsBlurred = value;
            }
        }
    }
}