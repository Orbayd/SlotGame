using SpykeGames.Showcase.Core.Enums;
using UnityEngine;

namespace SpykeGames.Showcase.Core.Column
{
    public class StoppingState : ColumnStateBase<SlotColumn>
    {
        public ColumnStateType State => ColumnStateType.Stoping;
        private ColumnConfig _config;
        private Vector3 _endPosition;
        private IColumnCommand _command;

        public StoppingState(SlotColumn data, ColumnConfig config, IColumnCommand command) : base(data)
        {
            _config = config;
            _command = command;
            _command.SetTime(0.001f * 100.0f);// Use From Config
        }
        public override void OnEnter()
        {
            _command.IsSmooth = true;
            _command.AllowLoop = false;
        }

        public override void OnUpdate(float deltaTime)
        {
            _command.Execute(Time.deltaTime);
            if (_command.IsFinished)
            {
                Data.ChangeState(ColumnStateType.Ready);
            }
        }

        public override void OnExit()
        {
            _command.IsSmooth = false;
        }

        public override void SetTarget(Vector2 target)
        {
            _endPosition = target;
        }
    }
}