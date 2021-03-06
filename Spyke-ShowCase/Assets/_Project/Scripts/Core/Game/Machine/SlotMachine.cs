using System;
using System.Collections;
using System.Linq;
using SpykeGames.Showcase.Core.Column;
using SpykeGames.Showcase.Core.Dealer;
using SpykeGames.Showcase.Core.Enums;
using SpykeGames.Showcase.Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace SpykeGames.Showcase.Core
{
    public class SlotMachine : MonoBehaviour
    {
        [SerializeField]
        private SlotColumn[] Columns;

        [Header("Config")]
        [SerializeField]
        private SlotMachineConfig _config;

        [Header("Particle")]
        [SerializeField]
        private ParticleComponent _particleSystem;

        [Header("UI")]
        [SerializeField]
        private Button _rollBtn;

        private SlotCombinationType _currentSlot;
        private DeckManager _deckManager;

        private bool _allowRoll = true;
        private bool AllowRoll { get { return _allowRoll; } set { _allowRoll = value; OnAllowRollChanged(value); } }

        

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
            AllowRoll = false;
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
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, delay));
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
            
            }
            else
            {
                AllowRoll = true;
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

        private void OnAllowRollChanged(bool value)
        {
            _rollBtn.interactable = value;
        }

        private void OnRollBtnClicked()
        {
            Roll();
        }

        private void AddEvents()
        {
            foreach (var item in Columns)
            {
                item.OnRollFinished += OnColumnRollFinshed;
            }

            _particleSystem.ParticleSystemStopped += OnParticleSystemStopped;

            _rollBtn.onClick.AddListener(OnRollBtnClicked);

        }

        private void RemoveEvents()
        {
            foreach (var item in Columns)
            {
                item.OnRollFinished -= OnColumnRollFinshed;
            }
            _particleSystem.ParticleSystemStopped -= OnParticleSystemStopped;

            _rollBtn.onClick.RemoveAllListeners();
        }

        #endregion

    }
}
