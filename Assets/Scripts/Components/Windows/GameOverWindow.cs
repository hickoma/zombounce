﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Windows
{
    public class GameOverWindow : MonoBehaviour
    {
		[SerializeField]
		private Button m_GetEnergyButton = null;

		[SerializeField]
		private GameObject m_NoInternetBadge = null;

		[SerializeField]
		private Button m_GetCoinsButton = null;

		[SerializeField]
		private Text m_KeepGoingText = null;

		[SerializeField]
		private Text m_TimerText = null;

        [SerializeField]
        private Button m_RestartButton = null;

		private bool m_AlreadyDied = false;
        private Coroutine m_TimerCoroutine = null;
        private Tween.Base m_EnergyButtonScaleTween = null;

		public void Start()
		{
            m_GetEnergyButton.onClick.AddListener(PlayMore);
			m_GetCoinsButton.onClick.AddListener(ClaimPrize);
            m_RestartButton.onClick.AddListener(ClaimPrize);
		}

		void OnEnable()
		{
			m_NoInternetBadge.SetActive (false);

			if (!m_AlreadyDied)
			{
				// track first death
				Analytics.SendEventAnalytic (Analytics.PossibleEvents.SessionHalfEnd, Systems.GameState.Instance.SessionsCount.ToString());

                bool isRewardVideoAvailable = Systems.GameState.Instance.IsRewardVideoAvailable;

				// show timer, captions and energy button
                m_GetEnergyButton.gameObject.SetActive (isRewardVideoAvailable);
				m_GetCoinsButton.gameObject.SetActive (false);
				m_KeepGoingText.gameObject.SetActive (true);
                m_TimerText.gameObject.SetActive (isRewardVideoAvailable);

                // show timer only if there is an alternative to restart - watching reward video
                if (isRewardVideoAvailable)
                {
                    StartTimer();
                    AnimateEnergyButton();
                }
			}
			else
			{
				// track second death
				Analytics.SendEventAnalytic (Analytics.PossibleEvents.SessionEnd, Systems.GameState.Instance.SessionsCount.ToString());

                // immediately go to Claim Prize Window
                ClaimPrize();
			}

			// count death
			m_AlreadyDied = true;
		}

		private void StartTimer()
		{
            m_TimerCoroutine = StartCoroutine (RunTimer ());
		}

		private IEnumerator RunTimer()
		{
			int timerCount = Systems.GameState.Instance.GameOverTimerCount;

			for (int i = timerCount; i > 0; i--)
			{
				m_TimerText.text = i.ToString ();
				yield return new WaitForSeconds (1);
			}

            ClaimPrize();
		}

		private void StopTimer()
		{
            StopCoroutine (m_TimerCoroutine);
		}

        private void AnimateEnergyButton()
        {
            Transform getEnergyTransform = m_GetEnergyButton.transform;
            m_EnergyButtonScaleTween = Tween.Value(1f).From(1f).To(1.3f).OnUpdate((v) =>
            {
                getEnergyTransform.localScale = Vector3.one * v;
            }).OnFinal(() =>
            {
                getEnergyTransform.localScale = Vector3.one;
            }).PingPong.Start();
        }

		private void PlayMore()
		{
            StopTimer ();

            if (m_EnergyButtonScaleTween != null)
            {
                m_EnergyButtonScaleTween.Stop();
            }

			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				m_NoInternetBadge.SetActive (false);

				GameEventsController.Instance.ShowRewardVideo (() =>
				{
					GameEventsController.Instance.PlayMore ();

					HideWindow ();
				});
			}
			else
			{
				m_NoInternetBadge.SetActive (true);
			}
		}

		private void ClaimPrize()
		{
            if (m_EnergyButtonScaleTween != null)
            {
                m_EnergyButtonScaleTween.Stop();
            }

			GameEventsController.Instance.StartRewarding();
			HideWindow();
		}

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
