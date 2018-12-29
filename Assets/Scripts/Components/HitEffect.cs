using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
	public class HitEffect : MonoBehaviour
	{
		[SerializeField]
		private Transform m_Transform = null;

		[SerializeField]
		private SpriteRenderer m_Icon = null;

		// parameters
		[System.NonSerialized]
		public float m_AnimationLength = 1f;
		[System.NonSerialized]
		public float m_AnimationDistanceDelta = 5f;

		void Start ()
		{
			// cache start position
			Vector3 startPosition = m_Transform.localPosition;
			// compute directional tangent
			float someAngleTan = Mathf.Tan (m_Transform.localRotation.eulerAngles.y * Mathf.Deg2Rad);
			// create direction vector
			Vector3 direction = new Vector3 (someAngleTan, 0, 1f);
			// normalize it
			direction.Normalize ();

			Tween.Value (m_AnimationLength).OnUpdate((v) =>
			{
				// move
				m_Transform.localPosition =  Vector3.Lerp(startPosition, startPosition + direction * m_AnimationDistanceDelta, v);
				// set opacity
				m_Icon.color = new Color(m_Icon.color.r, m_Icon.color.g, m_Icon.color.b, Mathf.Lerp(1f, 0f, v));
			}).OnComplete(() =>
			{
				Destroy(gameObject);
			}).Target(this).Start ();
		}
	}
}