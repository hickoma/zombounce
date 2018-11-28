using Components;
using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class GameOverProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

//        private EcsFilter<PlayerDeathEvent> _deathEvent = null;
		private Player m_Player = null;

        private bool _alreadyDead = false;

        private Transform _takeCoins;

        public GameObject GameOverPanel;
        public int TimerCount;

        public void Run()
        {
			
        }

        public void Initialize()
        {
			GameEventsController.Instance.OnPlayerDead += GameOver;
            _takeCoins = GameOverPanel.transform.FindRecursiveByTag(Tag.TakeEnergy);
        }

        public void Destroy()
        {
            GameOverPanel = null;
        }

		private void GameOver()
		{
			Pause();
			SetMenuEnabled();
			SaveBestScore();
			_alreadyDead = true;
		}

        private void Pause()
        {
			GameEventsController.Instance.ChangeGameState (Systems.GameState.State.GAME_OVER);
        }

        private void SetMenuEnabled()
        {
            if (_alreadyDead)
            {
                GameOverPanel.SetActive(true);
                _takeCoins.gameObject.SetActive(false);
                _ecsWorld.CreateEntityWith<ShowTakeCoinsEvent>();
            }
            else
            {
                GameOverPanel.SetActive(true);
                _takeCoins.gameObject.SetActive(true);
                var timer = _ecsWorld.CreateEntityWith<StartStopTimerEvent>();
                timer.IsStart = true;
                timer.Count = TimerCount;
            }
        }

        private void SaveBestScore()
        {
            GameEventsController.Instance.SaveScore();
        }
    }
}