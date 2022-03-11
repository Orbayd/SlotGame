
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class ReadyState : ColumnStateBase<Column>
    {
        public ColumnStateType State => ColumnStateType.Ready;
        public ReadyState(Column data, ColumnConfig config) : base(data)
        {

        }
        public override void OnEnter()
        {

        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnExit()
        {
            
        }
    }
}