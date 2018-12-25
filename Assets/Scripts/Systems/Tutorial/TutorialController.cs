using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
	public class TutorialController : MonoBehaviour
	{
		[SerializeField]
		private Components.Player m_Player = null;

		[SerializeField]
		private SpriteRenderer m_Hand = null;

		[SerializeField]
		private SpriteMask m_RayMask = null;

		[Space]
		[Header("Animation Parameters")]
		public int m_TutorialShowTimes;
		public float m_FirstPartLength;
		public float m_SecondPartLength;
		public float m_ThirdPartLength;

		private const float m_MaskMaxSize = 800f;

		public void LateStart()
		{
			if (Systems.GameState.Instance.SessionsCount <= m_TutorialShowTimes)
			{
				StartTutorial ();
				GameEventsController.Instance.OnGameStartClick += StopTutorial;
			}
		}

		void StartTutorial()
		{
			m_Hand.gameObject.SetActive (true);

			Tween.Value (m_FirstPartLength + m_SecondPartLength + m_ThirdPartLength).From(0f).To(m_FirstPartLength + m_SecondPartLength + m_ThirdPartLength).OnUpdate ((v) =>
			{
				if (v < m_FirstPartLength) // first part: tap
				{
					// set hand position
					m_Hand.transform.localPosition = new Vector3(m_Hand.transform.localPosition.x, 
																	0, 
																	m_Hand.transform.localPosition.z);
					// scale hand
					m_Hand.transform.localScale = Vector3.one * Mathf.Lerp(1.5f, 
																			1.2f, 
																			v / m_FirstPartLength);
					// set ray scale
					m_RayMask.transform.localScale = new Vector3(m_RayMask.transform.localScale.x, 
																m_MaskMaxSize, 
																m_RayMask.transform.localScale.z);
				}
				else if (v >= m_FirstPartLength && v <= m_FirstPartLength + m_SecondPartLength) // second part: drag
				{
					// move hand
					m_Hand.transform.localPosition = new Vector3(m_Hand.transform.localPosition.x, 
																Mathf.Lerp(0, -3, (v - m_FirstPartLength) / m_SecondPartLength), 
																m_Hand.transform.localPosition.z);
					// scale ray
					m_RayMask.transform.localScale = new Vector3(m_RayMask.transform.localScale.x, 
																Mathf.Lerp(m_MaskMaxSize, 0f, (v - m_FirstPartLength) / m_SecondPartLength), 
																m_RayMask.transform.localScale.z);
				}
				else if (v > m_FirstPartLength + m_SecondPartLength) // third part: release
				{
					// scale hand
					m_Hand.transform.localScale = Vector3.one * Mathf.Lerp(1.2f, 
																			1.5f, 
																			(v - m_FirstPartLength - m_SecondPartLength) / m_ThirdPartLength);

					// set ray scale
					m_RayMask.transform.localScale = new Vector3(m_RayMask.transform.localScale.x, 
						m_MaskMaxSize, 
						m_RayMask.transform.localScale.z);
				}
			}).Target(this).SetLoop(Tween.LoopType.loop).Start ();
		}

		void StopTutorial()
		{
			Tween.Stop (this);

			// hide hand
			m_Hand.gameObject.SetActive (false);

			// set mask original size
			m_RayMask.transform.localScale = new Vector3(m_RayMask.transform.localScale.x, 
															m_MaskMaxSize, 
															m_RayMask.transform.localScale.z);

			// unsubscribe from event
			GameEventsController.Instance.OnGameStartClick -= StopTutorial;
		}
	}
}
