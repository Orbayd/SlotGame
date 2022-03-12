
using SpykeGames.Showcase.Core.Enums;
using UnityEngine;

namespace SpykeGames.Showcase.Core.Column
{
    public class ReadyState : ColumnStateBase<SlotColumn>
    {
        public ColumnStateType State => ColumnStateType.Ready;
        public ReadyState(SlotColumn data, ColumnConfig config) : base(data)
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