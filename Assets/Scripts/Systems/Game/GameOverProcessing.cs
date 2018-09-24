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
            if (_deathEvent.EntitiesCount > 0)
            {
                Pause();
                SetMenuEnabled();
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
    }
}