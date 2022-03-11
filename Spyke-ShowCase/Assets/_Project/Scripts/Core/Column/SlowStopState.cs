using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class StoppingState : ColumnStateBase<Column>
    {
        public ColumnStateType State => ColumnStateType.Stoping;
        private ColumnConfig _config;
        private Vector3 _endPosition;
        private IColumnCommand _command;

        public StoppingState(Column data, ColumnConfig config, IColumnCommand command) : base(data)
        {
            _config = config;
            _command = command;
        }
        public override void OnEnter()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
           _command.Execute(Time.deltaTime);
           if(_command.IsFinished())
           {
               Data.ChangeState(ColumnStateType.Ready);
           }
        }

        public override void OnExit()
        {

        }

        public override void SetTarget(Vector3 target)
        {
            _endPosition = target;
        }
    }
}