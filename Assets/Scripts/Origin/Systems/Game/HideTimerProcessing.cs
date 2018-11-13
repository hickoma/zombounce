using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Game
{
    [EcsInject]
    public class HideTimerProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<HideTimerEvent> _hideTimerEventFilter;

        private Transform _takeEnergy;
        private Button _button;
        private Vector3 _rescaleVector;

        private bool _isPlaying;

        public GameObject GameOverPanel;
        public float RescaleSpeed;

        public void Initialize()
        {
            _takeEnergy = GameOverPanel.transform.FindRecursiveByTag(Tag.TakeEnergy);
            _button = _takeEnergy.FindRecursiveByTag(Tag.Button).GetComponent<Button>();
            _rescaleVector = new Vector3(RescaleSpeed, RescaleSpeed, RescaleSpeed);
        }

        public void Destroy()
        {
            _takeEnergy = null;
        }

        public void Run()
        {
            if (_isPlaying)
            {
                if (_takeEnergy.localScale.x <= 0f)
                {
                    _takeEnergy.gameObject.SetActive(false);
                    _ecsWorld.CreateEntityWith<ShowTakeCoinsEvent>();
                    _isPlaying = false;
                    _takeEnergy.localScale = Vector3.one;
                    _button.enabled = true;
                }
                else
                {
                    _takeEnergy.localScale -= _rescaleVector * Time.unscaledDeltaTime;
                }
            }
            else
            {
                for (int i = 0; i < _hideTimerEventFilter.EntitiesCount; i++)
                {
                    _isPlaying = true;
                    _ecsWorld.RemoveEntity(_hideTimerEventFilter.Entities[i]);
                    _button.enabled = false;
                }
            }
        }
    }
}