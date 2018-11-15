using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems
{
	public class DistanceBonusController : MonoBehaviour
    {
        private EcsWorld _world = null;
		private Player m_Player = null;

        private int _traveledDistance;

		public void LateStart()
		{
			_traveledDistance = 0;

			GameEventsController.Instance.OnDistanceChanged += CheckDistance;
		}

		private void CheckDistance(float distance)
        {
			if (distance > _traveledDistance)
            {
				int addPoints = Mathf.FloorToInt(distance - _traveledDistance);
				_traveledDistance = Mathf.FloorToInt(distance);

				if (_world == null)
				{
					_world = EcsWorld.Active;
				}

				_world.CreateEntityWith<AddPointsEvent>().Points = addPoints;
            }
        }
    }
}
