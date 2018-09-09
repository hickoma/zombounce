using Components;
using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class GameOverProcessing : IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        
        private EcsFilter<PlayerDeathEvent> _deathEvent = null;
        private EcsFilter<Player> _player = null;
        
        public GameObject Menu;
        
        public void Run()
        {
            if (_deathEvent.EntitiesCount > 0)
            {
                Pause();
                SetMenuEnabled();
            }
        }

        private void Pause()
        {
            Time.timeScale = 0f;
        }

        private void SetMenuEnabled()
        {
            Menu.SetActive(true);
        }

       
    }
}