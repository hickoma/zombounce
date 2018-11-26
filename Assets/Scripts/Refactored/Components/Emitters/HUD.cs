using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class HUD : MonoBehaviour
    {
        [Header("Buttons")]
		[SerializeField]
		private Button m_PauseButton = null;

		[SerializeField]
		private Button m_SettingsButton = null;

		[Header("Indicators")]
		[SerializeField]
		private Text m_CoinsIndicator = null;

        [Space]
        [Header("Windows")]
        [SerializeField]
        private GameObject m_PauseWindow = null;

        [SerializeField]
        private GameObject m_FistStoreWindow = null;

		public void LateStart()
		{
            GameEventsController.Instance.OnGameStateChanged += OnGameStateChanged;

            m_PauseButton.gameObject.SetActive(false);
			m_PauseButton.onClick.AddListener (GameEventsController.Instance.PauseGame);

//			m_SettingsButton.onClick.AddListener (GameEventsController.Instance.OpenSettings);

			// init coins
			UpdateCoins(Systems.GameState.Instance.CoinsCount);
			Systems.GameState.Instance.OnCoinsChanged += UpdateCoins;
		}

        void OnGameStateChanged(Components.Events.GameState newState)
        {
            SetPauseButtonState(newState);
            SetPauseWindowState(newState);
        }

        private void SetPauseButtonState(Components.Events.GameState currentState)
        {
            switch (currentState)
            {
                case Components.Events.GameState.PLAY:
                    m_PauseButton.gameObject.SetActive(true);
                    break;

                case Components.Events.GameState.GAME_OVER:
                case Components.Events.GameState.PAUSE:
                    m_PauseButton.gameObject.SetActive(false);
                    break;
            }
        }

        private void SetPauseWindowState(Components.Events.GameState currentState)
        {
            switch (currentState)
            {
                case Components.Events.GameState.PLAY:
                case Components.Events.GameState.GAME_OVER:
                    m_PauseWindow.SetActive(false);
                    break;
                case Components.Events.GameState.PAUSE:
                    GameEventsController.Instance.SaveScore();
                    m_PauseWindow.SetActive(true);
                    break;
            }
        }

		private void UpdateCoins(int count)
		{
			m_CoinsIndicator.text = count.ToString ();
		}
    }
}
