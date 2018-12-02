using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class HUD : MonoBehaviour
    {
        [Header("Buttons")]
		[SerializeField]
		private Button m_PauseButton = null;

		[Header("Indicators")]
		[SerializeField]
		private Text m_CoinsIndicator = null;

		[SerializeField]
		private Text m_TurnsIndicator = null;

        [Space]
        [Header("Windows")]
		[SerializeField]
		private GameObject m_StartGameWindow = null;

        [SerializeField]
        private GameObject m_PauseWindow = null;

        [SerializeField]
        private GameObject m_FistStoreWindow = null;

		public void LateStart()
		{
            GameEventsController.Instance.OnGameStateChanged += OnGameStateChanged;

            m_PauseButton.gameObject.SetActive(false);
			m_PauseButton.onClick.AddListener (GameEventsController.Instance.PauseGame);

			// init coins
			UpdateCoins(Systems.GameState.Instance.CoinsCount);
			Systems.GameState.Instance.OnCoinsChanged += UpdateCoins;

			// init turns
			UpdateTurns(Systems.GameState.Instance.TurnsCount);
			Systems.GameState.Instance.OnTurnsChanged += UpdateTurns;

			// show Start Game Window
			m_StartGameWindow.SetActive (true);
		}

		void OnGameStateChanged(Systems.GameState.State newState)
        {
            SetPauseButtonState(newState);
            SetPauseWindowState(newState);
        }

		private void SetPauseButtonState(Systems.GameState.State currentState)
        {
            switch (currentState)
            {
				case Systems.GameState.State.PLAY:
                    m_PauseButton.gameObject.SetActive(true);
                    break;

				case Systems.GameState.State.GAME_OVER:
				case Systems.GameState.State.PAUSE:
                    m_PauseButton.gameObject.SetActive(false);
                    break;
            }
        }

		private void SetPauseWindowState(Systems.GameState.State currentState)
        {
            switch (currentState)
            {
				case Systems.GameState.State.PLAY:
				case Systems.GameState.State.GAME_OVER:
                    m_PauseWindow.SetActive(false);
                    break;
				case Systems.GameState.State.PAUSE:
                    GameEventsController.Instance.SaveScore();
                    m_PauseWindow.SetActive(true);
                    break;
            }
        }

		private void UpdateCoins(int count)
		{
			m_CoinsIndicator.text = count.ToString();
		}

		private void UpdateTurns(int count)
		{
			m_TurnsIndicator.text = count.ToString();
		}
    }
}
