using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Game
{
    [EcsInject]
    public class ShowTakeCoinsProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<ShowTakeCoinsEvent> _showTimerEventFilter;
        
        private Transform _takeCoins;
        private Button _button;
        
        private Vector3 _rescaleVector;
        private bool _isPlaying;
        
        public GameObject GameOverPanel;
        public float RescaleSpeed;
        
        public void Initialize()
        {
//            _takeCoins = GameOverPanel.transform.FindRecursiveByTag(Tag.TakeCoins);
//            _button = _takeCoins.GetComponent<Button>();
            _rescaleVector = new Vector3(RescaleSpeed, RescaleSpeed, RescaleSpeed);
        }

        public void Destroy()
        {
//            _takeCoins = null;
        }

        public void Run()
        {
            if (_isPlaying)
            {
//                if (_takeCoins.localScale.x >= 1f)
//                {
                    _isPlaying = false;
//                    _takeCoins.localScale = Vector3.one;
//                    _button.enabled = true;
//                }
//                else
//                {
//                    _takeCoins.localScale += _rescaleVector * Time.unscaledDeltaTime;
//                }
            }
            else
            {
                for (int i = 0; i < _showTimerEventFilter.EntitiesCount; i++)
                {
//                    _takeCoins.gameObject.SetActive(true);
                    _isPlaying = true;
                    _ecsWorld.RemoveEntity(_showTimerEventFilter.Entities[i]);
//                    _button.enabled = false;
                }
            }
        }
    }
}