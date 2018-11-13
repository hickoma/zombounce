﻿using Components;
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
        private EcsFilter<AddPointsEvent> _addPointsEventFilter = null;
        private EcsFilter<BestScore> _bestScoreFilter = null;
        private EcsFilter<SaveScoreEvent> _saveScoreEventFilter = null;
        private EcsFilter<UpdateScoreEvent> _updateScoreEventFilter = null;

        private int _currentPoints;

        public void Initialize()
        {
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
            CheckUpdatePoints();
            CheckSaveBestScore();
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

        private void CheckSaveBestScore()
        {
            for (int i = 0; i < _saveScoreEventFilter.EntitiesCount; i++)
            {
                var currentBestScore = PlayerPrefs.GetInt(PrefKeys.BestScoreKey);
                if (_currentPoints > currentBestScore)
                {
                    PlayerPrefs.SetInt(PrefKeys.BestScoreKey, _currentPoints);
                    PlayerPrefs.Save();
                }
                
                _world.RemoveEntity(_saveScoreEventFilter.Entities[i]);
            }
        }

        private void CheckUpdatePoints()
        {
            var oldPoints = _currentPoints;
            for (int i = 0; i < _addPointsEventFilter.EntitiesCount; i++)
            {
                _currentPoints += _addPointsEventFilter.Components1[i].Points;
                _world.RemoveEntity(_addPointsEventFilter.Entities[i]);
            }

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