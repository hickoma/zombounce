using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class PauseButtonStateProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        private EcsFilter<GameStateEvent> _gameStateEvent = null;

        private GameObject _pauseButton;

        public void Initialize()
        {
            _pauseButton = GameObject.FindGameObjectWithTag(Tag.PauseButton);
            _pauseButton.SetActive(false);
        }

        public void Destroy()
        {
            _pauseButton = null;
        }

        public void Run()
        {
            for (int i = 0; i < _gameStateEvent.EntitiesCount; i++)
            {
                SetState(_gameStateEvent.Components1[i].State);
            }
        }

        private void SetState(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.PLAY:
                    _pauseButton.SetActive(true);
                    break;

                case GameState.GAME_OVER:
                case GameState.PAUSE:
                    _pauseButton.SetActive(false);
                    break;
            }
        }
    }
}