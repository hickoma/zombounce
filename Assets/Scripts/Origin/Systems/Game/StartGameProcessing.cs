using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Systems.Game
{
    [EcsInject]
    public class StartGameProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        private EcsFilter<GameStateEvent> _gameStateEvent = null;

        public GameObject StartGamePanel;

        public void Initialize()
        {
            
        }

        public void Destroy()
        {
            StartGamePanel = null;
        }

        public void Run()
        {
            for (int i = 0; i < _gameStateEvent.EntitiesCount; i++)
            {
				if (_gameStateEvent.Components1[i].State == Components.Events.GameState.PLAY)
                {
                    StartGamePanel.SetActive(false);
                }
            }
        }
    }
}