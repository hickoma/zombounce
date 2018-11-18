using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class BonusProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<Points> _pointsFilter = null;
        private EcsFilter<BestScore> _bestScoreFilter = null;
//        private EcsFilter<SaveScoreEvent> _saveScoreEventFilter = null;
        private EcsFilter<UpdateScoreEvent> _updateScoreEventFilter = null;

        private int _currentPoints;

//		public void LateStart()
//		{
//			GameEventsController.Instance.OnPointsAdded += AddPoints;
//          GameEventsController.Instance.OnScoreSaved += SaveBestScore;
//		}

        public void Initialize()
        {
			GameEventsController.Instance.OnPointsAdded += AddPoints;
            GameEventsController.Instance.OnScoreSaved += SaveBestScore;
            InitCurrentPoints();
            InitBestScore();
        }

        public void Destroy()
        {
            for (int i = 0; i < _pointsFilter.EntitiesCount; i++)
            {
                _pointsFilter.Components1[i].Text = null;
            }
        }

        public void Run()
        {
//            CheckUpdatePoints();
//            CheckSaveBestScore();
            CheckUpdateBestScore();
        }

        private void CheckUpdateBestScore()
        {
            for (int i = 0; i < _updateScoreEventFilter.EntitiesCount; i++)
            {
                UpdateBestScore();
                _world.RemoveEntity(_updateScoreEventFilter.Entities[i]);
            }
        }

        private void SaveBestScore()
        {
            int currentBestScore = PlayerPrefs.GetInt(PrefKeys.BestScoreKey);

            if (_currentPoints > currentBestScore)
            {
                PlayerPrefs.SetInt(PrefKeys.BestScoreKey, _currentPoints);
                PlayerPrefs.Save();
            }
        }

		private void AddPoints(int points)
        {
            int oldPoints = _currentPoints;
			_currentPoints += points;

            if (oldPoints != _currentPoints)
            {
                SetPoints();
            }
        }

        private void SetPoints()
        {
            for (int i = 0; i < _pointsFilter.EntitiesCount; i++)
            {
                _pointsFilter.Components1[i].Text.text = _currentPoints.ToString();
            }
        }

        private void InitBestScore()
        {
            var unityObject = GameObject.FindGameObjectWithTag(Tag.BestScore);
            var text = unityObject.GetComponent<Text>();
            var bestScore = _world.CreateEntityWith<BestScore>();
            bestScore.Text = text;
            _world.CreateEntityWith<UpdateScoreEvent>();
        }

        private void UpdateBestScore()
        {
            for (int i = 0; i < _bestScoreFilter.EntitiesCount; i++)
            {
                var text = _bestScoreFilter.Components1[i].Text;
                var bestScoreValue = PlayerPrefs.GetInt(PrefKeys.BestScoreKey);
                text.text =  "Best: " + bestScoreValue;
            }
        }

        private void InitCurrentPoints()
        {
            _currentPoints = 0;
            var unityObject = GameObject.FindGameObjectWithTag(Tag.Points);
            var text = unityObject.GetComponent<Text>();
            var points = _world.CreateEntityWith<Points>();
            points.Text = text;
            points.Text.text = "0";
        }
    }
}