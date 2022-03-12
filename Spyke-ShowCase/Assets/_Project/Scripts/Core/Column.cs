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
        private RollCommand _rollCommand;
        private ColumnStateType _currentState = ColumnStateType.Ready;
        private Dictionary<SlotType, Vector3> _typePositonMap = new Dictionary<SlotType, Vector3>();
        private Dictionary<ColumnStateType, IColumnState> _stateMap = new Dictionary<ColumnStateType, IColumnState>();
        
        public void Roll(SlotType type)
        {
            if (_currentState != ColumnStateType.Ready)
            {
                return;
            }

            ChangeState(ColumnStateType.Rolling);
            SetTarget(type);
        }
        public void Stop()
        {
            ChangeState(ColumnStateType.Stoping);
        }
        public void SetTarget(SlotType type)
        {
            var temp = _typePositonMap[type];
            temp.y *= -1;
            _stateMap[_currentState].SetTarget(temp);
        }
        public void ChangeState(ColumnStateType state)
        {
            // Debug.Log($"[INFO] State Changed to : {state}");
            _stateMap[_currentState].OnExit();

            if(_currentState == ColumnStateType.Stoping)
            {
                OnRollFinished?.Invoke(ColumnId);
            }

            _currentState = state;
            _stateMap[_currentState].OnEnter();
        }
        private void InitStates()
        {
            // _stateMap.Add(ColumnStateType.Ready,new ReadyState(this,_config));
            // _stateMap.Add(ColumnStateType.Rolling,new RollState(this,_config,_rollCommand));
            // _stateMap.Add(ColumnStateType.Stoping,new StoppingState(this,_config,_rollCommand));
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
        private void Start()
        {         
            _typePositonMap = Cells.Select(x => new { Type = x.CellType, Position = x.transform.position })
                                  .ToDictionary(x => x.Type, x => x.Position);
            _rollCommand = new RollCommand(transform,_config.DefaultRollTime,_config.MaxLimit);

            InitCells();
            InitStates();
        }
        private void FixedUpdate()
        {
            _stateMap[_currentState].OnUpdate(Time.deltaTime); 
        }

    }
}