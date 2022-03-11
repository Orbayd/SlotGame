using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpykeGames.Showcase.Core.Dealer;
using UnityEngine;

namespace SpykeGames.Showcase.Core
{
    public class SlotMachine : MonoBehaviour
    {
        [SerializeField]
        private Column[] Columns;

        [SerializeField]
        private SlotMachineConfig _config;

        [SerializeField]
        private ParticleComponent _particleSystem;
        private SlotCombinationType _currentSlot;
        private DeckManager _deckManager;
        private bool AllowRoll = true;
        void Start()
        {
            _deckManager = new DeckManager();
            AddEvents();
        }
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
            var targets = SlotCombination.GetOrCreate(_currentSlot).Slots;
            Debug.Log("[INFO] Target:" + string.Join(",",targets));
            for (int i = 0; i < Columns.Length; i++)
            {
                var column = Columns.ElementAt(i);
                column.Roll(targets[i]);
                yield return new WaitForSeconds(delay);
            }
        }

        private void GiveRevards()
        {
            var slotCombination = SlotCombination.GetOrCreate(_currentSlot);
            if (slotCombination.Reward > 0)
            {
                _particleSystem.Play(slotCombination.Reward);
                AllowRoll = false;
            }
            _deckManager.Dequeue();
        }
        private void OnColumnRollFinshed(int columnId)
        {
            var current = Columns.ElementAt(columnId);
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
                StartCoroutine(DelayedAction(_config.GetStopDelay(StopDelayType.Immediate), () => next.Stop()));
            }
            else if (next.ColumnId == 2)
            {
                StartCoroutine(DelayedAction(_config.GetStopDelay(SlotCombination.GetOrCreate(_currentSlot).StopDelay), 
                                            () => next.Stop()));
            }
        }

        IEnumerator DelayedAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
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

        void OnApplicationQuit()
        {
            _deckManager.Save();
        }
    }
}
