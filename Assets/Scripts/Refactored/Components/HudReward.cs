using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	public abstract class HudReward : MonoBehaviour
	{
		public int m_RewardAmount = 1;

		private Vector2 m_StartPosition;
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
			// canvas is parent of HUD that is parent to this
			RectTransform canvasRect = transform.parent.parent.gameObject.GetComponent<RectTransform>();
			Vector3 viewportPosition = Camera.main.WorldToViewportPoint(startPosition);
			Vector2 screenPosition = new Vector2 (viewportPosition.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x * 0.5f, 
				viewportPosition.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y * 0.5f);
			m_StartPosition = screenPosition;

			m_TargetPosition = targetPosition;
		}

		public virtual void GiveReward()
		{
			Destroy(gameObject);
		}
	}
}
