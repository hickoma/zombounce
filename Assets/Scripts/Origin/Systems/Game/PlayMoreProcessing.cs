using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class PlayMoreProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<PlayMoreEvent> _playMoreEventFilter;

        public GameObject GameOverPanel;

        public void Initialize()
        {
            
        }

        public void Destroy()
        {
            GameOverPanel = null;
        }

        public void Run()
        {
            for (int i = 0; i < _playMoreEventFilter.EntitiesCount; i++)
            {
                var playEvent = _playMoreEventFilter.Components1[i];
                SetMenuDisabled();
                SetPlayerAliveSprite();
                AddEnergy(playEvent.Energy);
                Play();
                _ecsWorld.CreateEntityWith<CountNowEvent>();
                _ecsWorld.RemoveEntity(_playMoreEventFilter.Entities[i]);
            }
        }

        private void AddEnergy(int energy)
        {
            _ecsWorld.CreateEntityWith<TurnChangedEvent>().Changed = energy;
        }

        private void Play()
        {
			GameEventsController.Instance.ChangeGameState (GameState.PLAY);
            var stateEvent = _ecsWorld.CreateEntityWith<GameStateEvent>();
            stateEvent.State = GameState.PLAY;
            _ecsWorld.CreateEntityWith<UpdateScoreEvent>();
        }

        private void SetMenuDisabled()
        {
            GameOverPanel.SetActive(false);
        }

        private void SetPlayerAliveSprite()
        {
			GameEventsController.Instance.SetPlayerSprite (true);
            _ecsWorld.CreateEntityWith<SetSprite>().isLive = true;
        }
    }
}