using System.Collections;
using System.Linq;
using SpykeGames.Showcase.Core.Column;
using SpykeGames.Showcase.Core.Dealer;
using SpykeGames.Showcase.Core.Enums;
using SpykeGames.Showcase.Core.Manager;
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class SlotMachine : MonoBehaviour
    {
        [SerializeField]
        private SlotColumn[] Columns;

        [SerializeField]
        private SlotMachineConfig _config;

        [SerializeField]
        private ParticleComponent _particleSystem;
        private SlotCombinationType _currentSlot;
        private DeckManager _deckManager;
        private bool AllowRoll = true;


        #region Unity Callbacks

        void Start()
        {
            _deckManager = new DeckManager();
            AddEvents();
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }

        void OnApplicationQuit()
        {
            _deckManager.Save();
        }

        #endregion

        #region Roll

        public void Roll()
        {
            if (!AllowRoll)
            {
                return;
            }
            _currentSlot = _deckManager.Peek();
            StartCoroutine(DelayedRoll(_config.DelayRange));
        }

        IEnumerator DelayedRoll(float delay)
        {
            var targets = CardCombination.GetOrCreate(_currentSlot).Slots;
            Debug.Log("[INFO] Target:" + string.Join(",",targets));
            for (int i = 0; i < Columns.Length; i++)
            {
                var column = Columns.ElementAt(i);
                column.Roll(targets[i]);
                //column.Roll(SlotType.Bonus);
                yield return new WaitForSeconds(delay);
            }
        }

        #endregion

        #region Rewards

        private void GiveRevards()
        {
            var slotCombination = CardCombination.GetOrCreate(_currentSlot);
            if (slotCombination.Reward > 0)
            {
                _particleSystem.Play(slotCombination.Reward);
                AllowRoll = false;
            }
            _deckManager.Dequeue();
        }

        #endregion

        #region Events

        private void OnColumnRollFinshed(int columnId)
        {
            if (columnId == Columns.Length - 1)
            {
                OnAllColumnsFinished();
                return;
            }
            StopNext(columnId);
        }

        private void StopNext(int columnId)
        {
            var next = Columns.ElementAt(++columnId);
            if (next.ColumnId == 1)
            {
                next.Stop(_config.GetStopDelay(StopDelayType.Immediate));
            }
            else if (next.ColumnId == 2)
            {
                next.Stop(_config.GetStopDelay(CardCombination.GetOrCreate(_currentSlot).StopDelay));
            }
        }

        private void OnAllColumnsFinished()
        {
            GiveRevards();
        }
        private void OnParticleSystemStopped()
        {
            AllowRoll = true;
        }
        private void AddEvents()
        {
            foreach (var item in Columns)
            {
                item.OnRollFinished += OnColumnRollFinshed;
            }

            _particleSystem.ParticleSystemStopped += OnParticleSystemStopped;

        }
        private void RemoveEvents()
        {
            foreach (var item in Columns)
            {
                item.OnRollFinished -= OnColumnRollFinshed;
            }
            _particleSystem.ParticleSystemStopped -= OnParticleSystemStopped;
        }

        #endregion

    }
}
