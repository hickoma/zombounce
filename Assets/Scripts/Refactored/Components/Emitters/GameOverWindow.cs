using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Windows
{
    public class GameOverWindow : MonoBehaviour
    {
		[SerializeField]
		private Button m_GetEnergyButton = null;

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
			if (!m_AlreadyDied)
			{
                bool isRewardVideoAvailable = Systems.GameState.Instance.IsRewardedVideoAvailable;

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

            GameEventsController.Instance.ShowAdvertising(() =>
            {
    			GameEventsController.Instance.PlayMore();

    			HideWindow();
            });
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
