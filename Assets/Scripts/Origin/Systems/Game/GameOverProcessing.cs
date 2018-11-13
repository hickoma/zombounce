﻿using Components;
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

        private EcsFilter<PlayerDeathEvent> _deathEvent = null;
        private EcsFilter<Player> _player = null;

        private bool _alreadyDead = false;

        private Transform _takeCoins;

        public GameObject GameOverPanel;
        public int TimerCount;

        public void Run()
        {
            for (int i = 0; i < _deathEvent.EntitiesCount; i++)
            {
                Pause();
                SetMenuEnabled();
                SetPlayerDeathSprite();
                SaveBestScore();
                _ecsWorld.RemoveEntity(_deathEvent.Entities[i]);
                _alreadyDead = true;
            }
        }

        public void Initialize()
        {
            _takeCoins = GameOverPanel.transform.FindRecursiveByTag(Tag.TakeEnergy);
        }

        public void Destroy()
        {
            GameOverPanel = null;
        }

        private void Pause()
        {
            var stateEvent = _ecsWorld.CreateEntityWith<GameStateEvent>();
            stateEvent.State = GameState.GAME_OVER;
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

        private void SetPlayerDeathSprite()
        {
            _ecsWorld.CreateEntityWith<SetSprite>().isLive = false;
        }

        private void SaveBestScore()
        {
            _ecsWorld.CreateEntityWith<SaveScoreEvent>();
        }
    }
}