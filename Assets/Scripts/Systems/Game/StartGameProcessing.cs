using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Systems.Game
{
    [EcsInject]
    public class StartGameProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        private EcsFilter<GameStateEvent> _gameStateEvent = null;

        private GameObject _pauseButton;
        private GameObject _settingsButton;

        public GameObject StartGamePanel;

        public void Initialize()
        {
            _pauseButton = GameObject.FindGameObjectWithTag(Tag.PauseButton);
            _settingsButton = GameObject.FindGameObjectWithTag(Tag.SettingsButton);
            Assert.IsNotNull(_pauseButton);
            Assert.IsNotNull(_settingsButton);

            _pauseButton.SetActive(false);
            _settingsButton.SetActive(true);

            _ecsWorld.CreateEntityWith<GameStateEvent>().State = GameState.NOT_INTERACTIVE;
        }

        public void Destroy()
        {
            _pauseButton = null;
            _settingsButton = null;
            StartGamePanel = null;
        }

        public void Run()
        {
            for (int i = 0; i < _gameStateEvent.EntitiesCount; i++)
            {
                if (_gameStateEvent.Components1[i].State == GameState.PLAY)
                {
                    StartGamePanel.SetActive(false);

                    //toggle button
                    _pauseButton.SetActive(true);
                    _settingsButton.SetActive(false);
                }
            }
        }
    }
}