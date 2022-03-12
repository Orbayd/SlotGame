
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class ReadyState : ColumnStateBase<ColumnView>
    {
        public ColumnStateType State => ColumnStateType.Ready;
        public ReadyState(ColumnView data, ColumnConfig config) : base(data)
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