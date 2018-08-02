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
        public Sprite DeathSprite;
        
        public void Run()
        {
            if (_deathEvent.EntitiesCount > 0)
            {
                Pause();
                SetMenuEnabled();
                SetPlayerDeadSprite();
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

        private void SetPlayerDeadSprite()
        {
            for (int i = 0; i < _player.EntitiesCount; i++)
            {
                var playerComponent = _player.Components1[i];
                playerComponent.SpriteRenderer.sprite = DeathSprite;
            }
        }
    }
}