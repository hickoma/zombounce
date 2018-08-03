using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class TurnCounterProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<TurnDecrementEvent> _turnDecrementEventFilter = null;
        private EcsFilter<TurnCounter> _turnCounterFilter = null;

        public int InitTurnCounter;

        public void Initialize()
        {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag(Tag.TurnCounter))
            {
                var text = unityObject.GetComponent<Text>();
                var turnCounter = _world.CreateEntityWith<TurnCounter>();
                turnCounter.TurnCountText = text;
                SetCountAndText(turnCounter, InitTurnCounter);
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < _turnCounterFilter.EntitiesCount; i++)
            {
                _turnCounterFilter.Components1[i].TurnCountText = null;
            }
        }

        public void Run()
        {
            if (_turnDecrementEventFilter.EntitiesCount > 0)
            {
                for (int i = 0; i < _turnCounterFilter.EntitiesCount; i++)
                {
                    var turnCounter = _turnCounterFilter.Components1[i];
                    var newCount = turnCounter.TurnCount - 1;
                    if (newCount < 0) return;
                    SetCountAndText(turnCounter, newCount);
                    if (newCount == 0)
                    {
                        _world.CreateEntityWith<PlayerDeathEvent>();
                    }
                }
            }
        }

        public void SetCountAndText(TurnCounter turnCounter, int count)
        {
            turnCounter.TurnCount = count;
            turnCounter.TurnCountText.text = count + "/" + InitTurnCounter;
        }
    }
}