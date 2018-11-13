using Components;
using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Game
{
    [EcsInject]
    public class TimerProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<StartStopTimerEvent> _startTimerEventFilter;

        private Timer _timer;
        private bool _isTimerRunning;

        private float _startTime;

        public GameObject GameOverPanel;

        public void Initialize()
        {
            var timerGo = GameOverPanel.transform.FindRecursiveByTag(Tag.Timer);
            var text = timerGo.GetComponent<Text>();
            _timer = _ecsWorld.CreateEntityWith<Timer>();
            _timer.TimerText = text;
        }

        public void Destroy()
        {
            GameOverPanel = null;
            if (_timer != null)
            {
                _timer.TimerText = null;
            }
        }

        public void Run()
        {
            if (_isTimerRunning)
            {
                CheckTimer();
            }

            for (int i = 0; i < _startTimerEventFilter.EntitiesCount; i++)
            {
                var startStopEvent = _startTimerEventFilter.Components1[i];
                if (startStopEvent.IsStart)
                {
                    StartTimer(startStopEvent.Count);
                }
                else
                {
                    StopTimer();
                }

                _ecsWorld.RemoveEntity(_startTimerEventFilter.Entities[i]);
            }
        }

        private void CheckTimer()
        {
            if (Time.unscaledTime - _startTime < 1f) return;

            _startTime = Time.unscaledTime;

            if (_timer.Time == 0)
            {
                _isTimerRunning = false;
                _ecsWorld.CreateEntityWith<HideTimerEvent>();
                return;
            }

            _timer.Time--;
            _timer.TimerText.text = _timer.Time.ToString();
            return;
        }

        private void StartTimer(int count)
        {
            _isTimerRunning = true;
            _startTime = Time.unscaledTime;

            _timer.Time = count;
            _timer.TimerText.text = count.ToString();
        }

        private void StopTimer()
        {
            _isTimerRunning = false;
        }
    }
}