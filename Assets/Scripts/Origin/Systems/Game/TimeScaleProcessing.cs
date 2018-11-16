using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    
    /**
     * TODO ПОдумать. Скорее всего стоит переделать на ивенты стейтов и глобальный обработчик, который будет юзать теже ивенты
     */
    [EcsInject]
    public class TimeScaleProcessing : IEcsRunSystem
    {
        private GameState _currentState = GameState.PAUSE;

        private EcsWorld _ecsWorld;
        private EcsFilter<GameStateEvent> _gameStateEventFilter = null;

        public void Run()
        {
            for (int i = 0; i < _gameStateEventFilter.EntitiesCount; i++)
            {
                _currentState = _gameStateEventFilter.Components1[i].State;
                SetState(_currentState);
            }
        }

        private void SetState(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.PLAY:
                    Time.timeScale = 1f;
                    break;

                case GameState.GAME_OVER:
                case GameState.PAUSE:
                    Time.timeScale = 0f;
                    break;
            }
        }
    }
}
