using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
            m_RestartButton.onClick.AddListener(RestartGame);
            m_GetEnergyButton.onClick.AddListener(PlayMore);
//			m_GetCoinsButton.onClick.AddListener(GameEventsController.Instance.StartGame);
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
				// show coins button
				m_GetEnergyButton.gameObject.SetActive (false);
				m_GetCoinsButton.gameObject.SetActive (true);
				m_KeepGoingText.gameObject.SetActive (false);
				m_TimerText.gameObject.SetActive (false);
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

			// show coins button
			m_GetEnergyButton.gameObject.SetActive (false);
			m_GetCoinsButton.gameObject.SetActive (true);
			m_KeepGoingText.gameObject.SetActive (false);
			m_TimerText.gameObject.SetActive (false);
		}

		private void StopTimer()
		{
			StopCoroutine ("RunTimer");
		}

		private void PlayMore()
		{
			StopTimer ();
			GameEventsController.Instance.PlayMore();

			HideWindow();
		}

		private void RestartGame()
		{
			StopTimer ();
			GameEventsController.Instance.RestartGame();

			SceneManager.LoadScene(0);
            HideWindow();
		}

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
