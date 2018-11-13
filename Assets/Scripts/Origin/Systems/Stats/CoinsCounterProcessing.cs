using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class CoinsCounterProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<CoinsChangedEvent> _coinsChangedEventFilter = null;
        private EcsFilter<CoinsCounter> _coinsCounterFilter = null;

        public void Initialize()
        {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag(Tag.CoinCounter))
            {
                var text = unityObject.GetComponent<Text>();
                var coinsCounter = _world.CreateEntityWith<CoinsCounter>();
                coinsCounter.CoinsCountText = text;
                SetCountAndText(coinsCounter, 0);
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < _coinsCounterFilter.EntitiesCount; i++)
            {
                _coinsCounterFilter.Components1[i].CoinsCountText = null;
            }
        }

        public void Run()
        {
            CheckTurnEvents();
        }

        private void CheckTurnEvents()
        {
            for (int i = 0; i < _coinsChangedEventFilter.EntitiesCount; i++)
            {
                var changed = _coinsChangedEventFilter.Components1[i].Changed;


                for (int j = 0; j < _coinsCounterFilter.EntitiesCount; j++)
                {
                    var coinsCounter = _coinsCounterFilter.Components1[j];
                    var newCount = coinsCounter.CoinsCount + changed;
                    if (newCount < 0) continue;

                    SetCountAndText(coinsCounter, newCount);
                }

                _world.RemoveEntity(_coinsChangedEventFilter.Entities[i]);
            }
        }

        private void SetCountAndText(CoinsCounter coinsCounter, int count)
        {
            coinsCounter.CoinsCount = count;
            coinsCounter.CoinsCountText.text = count.ToString();
        }
    }
}