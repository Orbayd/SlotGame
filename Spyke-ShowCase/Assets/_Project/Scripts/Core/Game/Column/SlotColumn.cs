using System;
using System.Collections.Generic;
using System.Linq;
using SpykeGames.Showcase.Core.Enums;
using UnityEngine;


namespace SpykeGames.Showcase.Core.Column
{
    public class SlotColumn : MonoBehaviour
    {
        public event Action<int> OnRollFinished;
        public int ColumnId;

        [SerializeField]
        private Slot[] _slotViews;

        [SerializeField]
        private ColumnConfig _config;

        private RollCommand _rollCommand;
        private Dictionary<SlotType, Vector3> _targetMap = new Dictionary<SlotType, Vector3>();
        private Dictionary<ColumnStateType, IColumnState> _stateMap = new Dictionary<ColumnStateType, IColumnState>();
        private ColumnStateType _currentState = ColumnStateType.Ready;

        #region Unity Callbacks

        private void Start()
        {
            InitCells();
            InitStates();
        }

        private void FixedUpdate()
        {
            _stateMap[_currentState].OnUpdate(Time.deltaTime);
        }

        #endregion

        #region Roll&Stop

        public void Roll(SlotType type)
        {
            if (_currentState != ColumnStateType.Ready)
            {
                return;
            }

            ChangeState(ColumnStateType.Rolling);
            SetTarget(type);

            _rollCommand.SetTime(_config.DefaultRollTime);
        }

        public void Stop(float time)
        {
            ChangeState(ColumnStateType.Stoping);

            _rollCommand.SetTime(time);
        }

        public void SetTarget(SlotType type)
        {
            var temp = _targetMap[type];
            temp.y *= -1;
            _stateMap[_currentState].SetTarget(temp);
        }

        #endregion

        public void ChangeState(ColumnStateType state)
        {
            _stateMap[_currentState].OnExit();

            if (_currentState == ColumnStateType.Stoping)
            {
                OnRollFinished?.Invoke(ColumnId);
            }

            _currentState = state;
            _stateMap[_currentState].OnEnter();
        }

        #region Init
        //Should use chain of responsibility?
        private void InitStates()
        {
            _rollCommand = new RollCommand(transform.GetComponent<RectTransform>(), _config.DefaultRollTime, _config.MaxLimit);

            _stateMap.Add(ColumnStateType.Ready, new ReadyState(this, _config));
            _stateMap.Add(ColumnStateType.Rolling, new RollState(this, _config, _rollCommand));
            _stateMap.Add(ColumnStateType.Stoping, new StoppingState(this, _config, _rollCommand));
        }

        private void InitCells()
        {
            var atlas = _config.Atlas;
            var cells = GetComponentsInChildren<Slot>();
            for (int i = 0; i < _config.SlotOrder.Length; i++)
            {
                var slotType = _config.SlotOrder[i];
                cells[i].Init(slotType, atlas.GetSprite(slotType.ToString()), atlas.GetSprite(slotType.ToString() + "_Blur"));
            }

            _targetMap = _slotViews.Select(x => new { Type = x._cellType, Position = CalculateTargetPosition(x) })
                                        .ToDictionary(x => x.Type, x => x.Position);
        }

        private Vector3 CalculateTargetPosition(Slot view)
        {
            var target = view.transform.parent.GetComponent<RectTransform>().anchoredPosition;
            target.y -= _config.MinLimit - 200;
            target.x = transform.GetComponent<RectTransform>().anchoredPosition.x;
            return target;
        }
        #endregion

    }
}