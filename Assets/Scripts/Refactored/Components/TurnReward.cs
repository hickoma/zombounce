using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	public class TurnReward : MonoBehaviour
	{
		public int m_RewardAmount = 1;

		private Vector3 m_StartPosition;
		private Vector3 m_TargetPosition;

		void Start ()
		{
			Transform thisTransform = transform;

			Tween.Value (0.5f).From(0f).To(0f).OnUpdate ((v) =>
			{
					float modX = Mathf.Lerp(m_StartPosition.x, m_TargetPosition.x, Tween.Ease.easeOutBack(v));
					float modY = Mathf.Lerp(m_StartPosition.y, m_TargetPosition.y, Tween.Ease.easeInOutCirc(v));
					thisTransform.position = new Vector3(modX, modY, thisTransform.position.z);
			}).Start ();
		}

		public void SetFlight(Vector3 startPosition, Vector3 targetPosition)
		{
			m_StartPosition = startPosition;
			m_TargetPosition = m_TargetPosition;
		}
	}
}
