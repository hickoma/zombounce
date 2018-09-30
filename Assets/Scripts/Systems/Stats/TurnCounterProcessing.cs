using System;
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
        private EcsFilter<TurnChangedEvent> _turnChangedEventFilter = null;
        private EcsFilter<TurnCounter> _turnCounterFilter = null;
        private EcsFilter<Player> _playerFilter = null;

        private bool _isGameOver = false;

        public int InitTurnCounter;
        public float MinVelocityTolerace;

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
            CheckDeathEvents();
            if (!_isGameOver)
            {
                CheckTurnEvents();
            }
        }

        private void CheckDeathEvents()
        {
            for (int i = 0; i < _turnCounterFilter.EntitiesCount; i++)
            {
                var currentCount = _turnCounterFilter.Components1[i].TurnCount;
                if (currentCount == 0)
                {
                    for (int k = 0; k < _playerFilter.EntitiesCount; k++)
                    {
                        if (Math.Abs(_playerFilter.Components1[i].Rigidbody.velocity.sqrMagnitude) <
                            MinVelocityTolerace)
                        {
                            _world.CreateEntityWith<PlayerDeathEvent>();
                        }
                    }
                }
            }
        }

        private void CheckTurnEvents()
        {
            for (int i = 0; i < _turnChangedEventFilter.EntitiesCount; i++)
            {
                var changed = _turnChangedEventFilter.Components1[i].Changed;


                for (int j = 0; j < _turnCounterFilter.EntitiesCount; j++)
                {
                    var turnCounter = _turnCounterFilter.Components1[j];
                    var newCount = turnCounter.TurnCount + changed;
                    if (newCount < 0 || newCount > InitTurnCounter) continue;

                    SetCountAndText(turnCounter, newCount);
                }

                _world.RemoveEntity(_turnChangedEventFilter.Entities[i]);
            }
        }

        public void SetCountAndText(TurnCounter turnCounter, int count)
        {
            turnCounter.TurnCount = count;
            turnCounter.TurnCountText.text = count + "/" + InitTurnCounter;
        }
    }
}