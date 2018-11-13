using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class DistanceBonusProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<DistanceEvent> _distanceEventFilter = null;
        private EcsFilter<Player> _playerFilter = null;

        private int _traveledDistance;

        public void Initialize()
        {
            _traveledDistance = 0;
        }

        public void Destroy()
        {
        }

        public void Run()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            for (int i = 0; i < _distanceEventFilter.EntitiesCount; i++)
            {
                var distanceEvent = _distanceEventFilter.Components1[i];
                var currentDistance = (int) distanceEvent.CurrentDistance;
                if (currentDistance > _traveledDistance)
                {
                    var addPoints = currentDistance - _traveledDistance;
                    _traveledDistance = currentDistance;
                    _world.CreateEntityWith<AddPointsEvent>().Points = addPoints;
                }

                _world.RemoveEntity(_distanceEventFilter.Entities[i]);
            }
        }
    }
}