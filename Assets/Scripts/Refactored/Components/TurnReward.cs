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
            RectTransform thisTransform = (RectTransform)transform;

			Tween.Value (0.5f).From(0f).To(1f).OnUpdate ((v) =>
			{
				float modX = Mathf.Lerp(m_StartPosition.x, m_TargetPosition.x, Tween.Ease.easeOutBack(v));
				float modY = Mathf.Lerp(m_StartPosition.y, m_TargetPosition.y, Tween.Ease.easeInOutCirc(v));
				thisTransform.anchoredPosition = new Vector3(modX, modY, thisTransform.position.z);
            }).OnComplete(GiveReward).Start ();
		}

		public void SetFlight(Vector3 startPosition, Vector3 targetPosition)
		{
            m_StartPosition = Camera.main.WorldToScreenPoint(startPosition);
			m_TargetPosition = targetPosition;
		}

        public void GiveReward()
        {
            Systems.GameState.Instance.TurnsCount += 1;
            Destroy(gameObject);
        }
	}
}
