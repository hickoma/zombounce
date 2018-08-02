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
        private EcsFilter<AddPointsEvent> _addPointsEventFilter = null;

        private int _currentPoints;

        public void Initialize()
        {
            _currentPoints = 0;
            var unityObject = GameObject.FindGameObjectWithTag(Tag.Points);
            var text = unityObject.GetComponent<Text>();
            var points = _world.CreateEntityWith<Points>();
            points.Text = text;
            points.Text.text = "0";
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
            CheckAddPoints();
        }

        private void CheckAddPoints()
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
    }
}