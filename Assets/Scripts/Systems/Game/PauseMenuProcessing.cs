using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class PauseMenuProcessing : IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<GameStateEvent> _gameStateEvent = null;

        public GameObject PausePanel;

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
                case GameState.GAME_OVER:
                    PausePanel.SetActive(false);
                    break;
                case GameState.PAUSE:
                    PausePanel.SetActive(true);
                    break;
            }
        }
    }
}