using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class HUD : MonoBehaviour
    {
		[SerializeField]
		private Button m_PauseButton = null;

		[SerializeField]
		private Button m_SettingsButton = null;

		public void LateStart()
		{
            GameEventsController.Instance.OnGameStateChanged += OnGameStateChanged;

            m_PauseButton.gameObject.SetActive(false);
			m_PauseButton.onClick.AddListener (GameEventsController.Instance.PauseGame);

//			m_SettingsButton.onClick.AddListener (GameEventsController.Instance.OpenSettings);
		}

        void OnGameStateChanged(Components.Events.GameState newState)
        {
            SetPauseButtonState(newState);
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
    }
}
