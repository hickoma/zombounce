using System;
using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class TurnCounterProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<CountNowEvent> _countNowEventFilter;

        private bool _isGameOver = false;

//		public void LateStart()
//		{
//			GameEventsController.Instance.OnTurnsChanged += ChangeTurns;
//			GameEventsController.Instance.OnPlayerStopped += CheckDeath;
//		}

        public void Initialize()
        {
			Systems.GameState.Instance.OnTurnsChanged += ChangeTurns;
			GameEventsController.Instance.OnPlayerStopped += CheckDeath;
        }

        public void Destroy()
        {
			
        }

        public void Run()
        {
//            CheckTurnEvents();

            if (!_isGameOver)
            {
//                CheckDeathEvents();
            }
            else
            {
                CheckPlayMoreEvent();
            }
        }

        private void CheckPlayMoreEvent()
        {
            for (int i = 0; i < _countNowEventFilter.EntitiesCount; i++)
            {
                _isGameOver = false;
                _world.RemoveEntity(_countNowEventFilter.Entities[i]);
            }
        }

		private void CheckDeath(float zPosition)
        {
			int currentCount = Systems.GameState.Instance.TurnsCount;

			// last turn and player stop - he's dead
            if (currentCount == 0)
            {
				GameEventsController.Instance.PlayerDie ();
                _isGameOver = true;
            }
        }

		private void ChangeTurns(int newCount)
		{
			if (newCount < 0)
			{
				GameEventsController.Instance.PlayerDie ();
				_isGameOver = true;
			}
		}
    }
}