using Components;
using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class GameOverProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<PlayerDeathEvent> _deathEvent = null;
        private EcsFilter<Player> _player = null;

        public GameObject GameOverPanel;

        public void Run()
        {
            for (int i = 0; i < _deathEvent.EntitiesCount; i++)
            {
                Pause();
                SetMenuEnabled();
                SetPlayerDeathSprite();
                _ecsWorld.RemoveEntity(_deathEvent.Entities[i]);
            }
        }

        public void Initialize()
        {
            //do nothing
        }

        public void Destroy()
        {
            GameOverPanel = null;
        }

        private void Pause()
        {
            var stateEvent = _ecsWorld.CreateEntityWith<GameStateEvent>();
            stateEvent.State = GameState.PAUSE;
        }

        private void SetMenuEnabled()
        {
            GameOverPanel.SetActive(true);
        }

        private void SetPlayerDeathSprite()
        {
            _ecsWorld.CreateEntityWith<SetDeathSprite>();
        }
    }
}