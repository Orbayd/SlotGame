using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SpykeGames.Showcase.Core
{
    public class Column : MonoBehaviour
    {
        [SerializeField]
        private Cell[] Cells;

        public int ColumnId;
        public event Action<int> OnRollFinished;

        [SerializeField]
        private ColumnConfig _config;

        //private float RollTime;
        //private float SpinTime;
        private Dictionary<SlotType, Vector3> _typePositonMap = new Dictionary<SlotType, Vector3>();
        private Dictionary<ColumnStateType, IColumnState> _stateMap = new Dictionary<ColumnStateType, IColumnState>();

        //Vector3 EndPosition;
        //Vector3 CurrentPosition;
        //Vector3 StartPosition;
        //Vector3 TargetPosition;

        private ColumnStateType _currentState = ColumnStateType.Ready;

        private RollCommand _rollCommand;

        public void Roll(SlotType type)
        {
            if (_currentState != ColumnStateType.Ready)
            {
                return;
            }

            // _currentState = ColumnStateType.Rolling;
            // CurrentPosition = transform.localPosition;
            // SpinTime = spinTime;
            // RollTime = _config.DefaultRollTime;

            // _rollCommand.SetSmootStep(false);
            //_rollCommand.SetTarget(StartPosition, EndPosition);

            ChangeState(ColumnStateType.Rolling);

            SetTarget(type);

            //SetCellSprites(true);
        }

        // private void SetCellSprites(bool value)
        // {
        //     var cells = GetComponentsInChildren<Cell>();
        //     foreach (var slot in cells)
        //     {
        //         slot.IsBlurred = value;
        //     }
        // }

        private void Start()
        {
           
            // var temp = this.transform.position;
            // temp.y = _config.MaxLimit;
            // var EndPosition = temp;

            // temp = this.transform.position;
            // temp.y = _config.MinLimit;
            // var StartPosition = temp;

            _typePositonMap = Cells.Select(x => new { Type = x.CellType, Position = x.transform.position })
                                  .ToDictionary(x => x.Type, x => x.Position);

            _rollCommand = new RollCommand(transform,_config.DefaultRollTime,_config.MaxLimit);
            //_rollCommand.SetTarget(new Vector3(temp.x,_config.MaxLimit,temp.y),new Vector3(temp.x,_config.MinLimit,temp.y));
            //_rollCommand.SetTarget(StartPosition, EndPosition);

            InitCells();
            InitStates();
        }

        private void InitStates()
        {
            _stateMap.Add(ColumnStateType.Ready,new ReadyState(this,_config));
            _stateMap.Add(ColumnStateType.Rolling,new RollState(this,_config,_rollCommand));
            _stateMap.Add(ColumnStateType.Stoping,new StoppingState(this,_config,_rollCommand));
        }

        public void ChangeState(ColumnStateType state)
        {
            Debug.Log($"[INFO] State Changed to : {state}");
            _stateMap[_currentState].OnExit();

            if(_currentState == ColumnStateType.Stoping)
            {
                OnRollFinished?.Invoke(ColumnId);
            }

            _currentState = state;
            _stateMap[_currentState].OnEnter();
        }
        private void InitCells()
        {
            var atlas = _config.Atlas;
            var cells = GetComponentsInChildren<Cell>();
            for (int i = 0; i < _config.SlotOrder.Length; i++)
            {
                var slotType  = _config.SlotOrder[i];
                cells[i].Init(slotType ,atlas.GetSprite(slotType.ToString()),atlas.GetSprite(slotType.ToString()+ "_Blur"));
            }
        }

        private void FixedUpdate()
        {
            // SpinTime -= Time.deltaTime;

            _stateMap[_currentState].OnUpdate(Time.deltaTime);
            // if (_currentState == ColumnStateType.Rolling) //Rolling
            // {
            //     if (SpinTime > 0)
            //     {                    
            //         _rollCommand.Execute(Time.deltaTime);
            //     }
            //     else
            //     {
            //         _currentState = ColumnStateType.Stoping;
            //         RollTime = _config.DefaultRollTime;
            //     }
            // }
            // else if (_currentState == ColumnStateType.Stoping)
            // {
            //     if (Vector3.Distance(transform.position ,TargetPosition) >= 0.5f) 
            //     {
            //         _rollCommand.Execute(Time.deltaTime);
            //     }
            //     else
            //     {
            //         transform.position = TargetPosition;
            //         _currentState = ColumnStateType.Stopped;
            //         OnRollFinished?.Invoke(ColumnId);

            //         _rollCommand.SetSmootStep(true);
            //         _rollCommand.SetTarget(transform.position,TargetPosition);
            //         SetCellSprites(false);
            //    }
            // }
            // else if (_currentState == ColumnStateType.Stopped) //Slowing Down & Stop
            // {
            //     //StopTurn();
            //      _rollCommand.Execute(Time.deltaTime);
            //      _currentState = ColumnStateType.Ready;
            // }

        }

        public void SetTarget(SlotType type)
        {
            var temp = _typePositonMap[type];
            temp.y *= -1;
            _stateMap[_currentState].SetTarget(temp);
        }

        private Cell FindCell(SlotType type)
        {
            return Cells.First(x => x.CellType == type);
        }

        public void Stop()
        {
            ChangeState(ColumnStateType.Stoping);
        }
    }
}