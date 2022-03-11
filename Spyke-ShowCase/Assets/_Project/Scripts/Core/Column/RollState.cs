
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class RollState : ColumnStateBase<Column>
    {
        public ColumnStateType State => ColumnStateType.Rolling;
        private ColumnConfig _config;
        private Vector3 _endPosition;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _spinTime;
        private float _initialSpinTime;
        private IColumnCommand _columnCommand;

        public RollState(Column data, ColumnConfig config ,IColumnCommand command) : base(data)
        {
            _config = config;
            _columnCommand = command;
            _initialSpinTime =  Data.ColumnId == 0 ? config.MaxSpintTime : float.MaxValue;
            
            var temp = Data.transform.position;
            temp.y = _config.MaxLimit;
            _endPosition = temp;

            temp = Data.transform.position;
            temp.y = _config.MinLimit;
            _startPosition = temp;

        }
        public override void OnEnter()
        {
            _spinTime = _initialSpinTime;
            _columnCommand.SetTarget(_startPosition,_endPosition);
            SetCellSprites(true);
        }

        public override void OnUpdate(float deltaTime)
        {
            _spinTime-=deltaTime;
            if(_spinTime > 0 )
            {
                _columnCommand.Execute(Time.deltaTime);
            }
            else if(Vector3.Distance(Data.transform.position, _targetPosition) >= 2.7f)
            {
                _columnCommand.Execute(Time.deltaTime);
                Debug.Log($"{Data.ColumnId} Last Roll: Distance{Vector3.Distance(Data.transform.position, _targetPosition)}");
            }
            else
                Data.ChangeState(ColumnStateType.Stoping);
        }
        public override void OnExit()
        {
            Data.transform.position = _targetPosition;
            _columnCommand.SetTarget(Data.transform.position,_targetPosition);
            SetCellSprites(false);
            _columnCommand.Reset();
        }
        public override void SetTarget(Vector3 target)
        {
            _targetPosition = target;
        }

        private void SetCellSprites(bool value)
        {
            var cells = Data.GetComponentsInChildren<Cell>();
            foreach (var slot in cells)
            {
                slot.IsBlurred = value;
            }
        }
    }
}