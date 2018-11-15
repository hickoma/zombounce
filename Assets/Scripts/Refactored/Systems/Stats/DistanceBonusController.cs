using Components;
using UnityEngine;

namespace Systems
{
	public class DistanceBonusController : MonoBehaviour
    {
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

				GameEventsController.Instance.AddPoints (addPoints);
            }
        }
    }
}
