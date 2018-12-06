using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Windows
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField]
        private Button m_RestartButton = null;

		[SerializeField]
		private Button m_GetEnergyButton = null;

		[SerializeField]
		private Button m_GetCoinsButton = null;

		[SerializeField]
		private Text m_KeepGoingText = null;

		[SerializeField]
		private Text m_TimerText = null;

		private bool m_AlreadyDied = false;

		public void Start()
		{			
			m_RestartButton.onClick.AddListener(ClaimPrize);
            m_GetEnergyButton.onClick.AddListener(PlayMore);
			m_GetCoinsButton.onClick.AddListener(ClaimPrize);
		}

		void OnEnable()
		{
			if (!m_AlreadyDied)
			{
				// show timer, captions and energy button
				m_GetEnergyButton.gameObject.SetActive (true);
				m_GetCoinsButton.gameObject.SetActive (false);
				m_KeepGoingText.gameObject.SetActive (true);
				m_TimerText.gameObject.SetActive (true);

				StartTimer ();
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
			StartCoroutine (RunTimer ());
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
			StopCoroutine ("RunTimer");
		}

		private void PlayMore()
		{
            GameEventsController.Instance.ShowAdvertising(() =>
            {
    			StopTimer ();
    			GameEventsController.Instance.PlayMore();

    			HideWindow();
            });
		}

		private void ClaimPrize()
		{
			GameEventsController.Instance.StartRewarding();
			HideWindow();
		}

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
