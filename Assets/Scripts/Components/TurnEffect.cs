using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
	public class TurnEffect : MonoBehaviour
	{
		[SerializeField]
		private RectTransform m_Transform = null;

		[SerializeField]
		private Image m_Icon = null;

		[SerializeField]
		private Text m_MinusText = null;

		private float m_AnimationLength = 1f;
		private float m_AnimationDeltaY = -100f;

		void Start ()
		{
			float startXPosition = m_Transform.anchoredPosition.x;
			float startYPosition = m_Transform.anchoredPosition.y;

			Tween.Value (m_AnimationLength).OnUpdate((v) =>
			{
				// move
				m_Transform.anchoredPosition = new Vector2(startXPosition, Mathf.Lerp(startYPosition, startYPosition + m_AnimationDeltaY, v));
				// set opacity
				m_Icon.color = new Color(m_Icon.color.r, m_Icon.color.g, m_Icon.color.b, Mathf.Lerp(1f, 0f, v));
			}).OnComplete(() =>
			{
				Destroy(gameObject);
			}).Target(this).Start ();
		}
	}
}