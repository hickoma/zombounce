using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class HideTimerProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<HideTimerEvent> _hideTimerEventFilter;

        private Transform _takeCoins;
        private Vector3 _rescaleVector;

        private bool _isPlaying;

        public GameObject GameOverPanel;
        public float RescaleSpeed;

        public void Initialize()
        {
            _takeCoins = GameOverPanel.transform.FindRecursiveByTag(Tag.TakeCoins);
            _rescaleVector = new Vector3(RescaleSpeed, RescaleSpeed, RescaleSpeed);
        }

        public void Destroy()
        {
            _takeCoins = null;
        }

        public void Run()
        {
            if (_isPlaying)
            {
                if (_takeCoins.localScale.x <= 0f)
                {
                    _takeCoins.gameObject.SetActive(false);
                    _ecsWorld.CreateEntityWith<ShowSimpleButtonEvent>();
                    _isPlaying = false;
                    _takeCoins.localScale = Vector3.one;
                }
                else
                {
                    _takeCoins.localScale -= _rescaleVector * Time.unscaledDeltaTime;
                }
            }
            else
            {
                for (int i = 0; i < _hideTimerEventFilter.EntitiesCount; i++)
                {
                    _isPlaying = true;
                    _ecsWorld.RemoveEntity(_hideTimerEventFilter.Entities[i]);
                }
            }
        }
    }
}