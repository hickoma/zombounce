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
        private Image m_CoinsIcon = null;

        [SerializeField]
        private Button m_AddCoinsButton = null;

		[SerializeField]
		private Text m_TurnsIndicator = null;

        [SerializeField]
        private Image m_TurnsIcon = null;

		[SerializeField]
		private Text m_PointsIndicator = null;

		[SerializeField]
		private Text m_BestScoreIndicator = null;

        [Space]
        [Header("Windows")]
		[SerializeField]
		private GameObject m_StartGameWindow = null;

        [SerializeField]
        private GameObject m_PauseWindow = null;

        [SerializeField]
        private GameObject m_FistStoreWindow = null;

        [SerializeField]
        private GameObject m_CoinsStoreWindow = null;

		[SerializeField]
		private GameObject m_GameOverWindow = null;

		[SerializeField]
		private GameObject m_ClaimPrizeWindow = null;

        [SerializeField]
        private GameObject m_AdvertisingWindow = null;

		[Space]
		[Header("Rewards")]
		[SerializeField]
		private Components.TurnReward m_TurnRewardPrefab = null;

		[SerializeField]
		private Components.CoinReward m_CoinRewardPrefab = null;

		public void LateStart()
		{
            GameEventsController.Instance.OnGameStateChanged += OnGameStateChanged;
            GameEventsController.Instance.OnShowAdvertising += OnShowAdvertising;
			GameEventsController.Instance.OnCreateRewardTurn += OnCreateRewardTurn;
			GameEventsController.Instance.OnCreateRewardCoin += OnCreateRewardCoin;

            m_PauseButton.gameObject.SetActive(false);
			m_PauseButton.onClick.AddListener (GameEventsController.Instance.PauseGame);

			// init coins
			UpdateCoins(Systems.GameState.Instance.CoinsCount);
			Systems.GameState.Instance.OnCoinsChanged += UpdateCoins;
            m_AddCoinsButton.onClick.AddListener(OpenCoinsStore);

			// init turns
			UpdateTurns(Systems.GameState.Instance.TurnsCount);
			Systems.GameState.Instance.OnTurnsChanged += UpdateTurns;

			// init points
			m_PointsIndicator.gameObject.SetActive (false);
			UpdatePoints(Systems.GameState.Instance.CurrentPointsCount);
			Systems.GameState.Instance.OnPointsChanged += UpdatePoints;

			// init best score
			UpdateBestScore(Systems.GameState.Instance.BestScorePointsCount);
			Systems.GameState.Instance.OnBestScoreChanged += UpdateBestScore;

			// show Start Game Window
			m_StartGameWindow.SetActive (true);
		}

		void OnCreateRewardTurn (Vector3 startPosition, int count)
		{
			Components.TurnReward turnReward = Instantiate (m_TurnRewardPrefab, transform) as Components.TurnReward;
			// icon position relatve to HUD
			Vector3 targetPosition = transform.InverseTransformPoint(m_TurnsIcon.rectTransform.position);
			turnReward.SetFlight (startPosition, targetPosition);
		}

		void OnCreateRewardCoin (Vector3 startPosition, int count)
		{
			Components.CoinReward coinReward = Instantiate (m_CoinRewardPrefab, transform) as Components.CoinReward;
			// icon position relatve to HUD
			Vector3 targetPosition = transform.InverseTransformPoint(m_CoinsIcon.rectTransform.position);
			coinReward.SetFlight (startPosition, targetPosition);
		}

		void OnGameStateChanged(Systems.GameState.State newState)
        {
            SetHudElementsState(newState);
            SetWindowsState(newState);
        }

        void OnShowAdvertising (System.Action onAdvertisingClose)
        {
            m_AdvertisingWindow.GetComponent<AdvertisingWindow>().Init(onAdvertisingClose);
            m_AdvertisingWindow.SetActive(true);
        }

        void OpenCoinsStore()
        {
            m_CoinsStoreWindow.SetActive(true);
        }

		private void SetHudElementsState(Systems.GameState.State currentState)
        {
            switch (currentState)
            {
                case Systems.GameState.State.PLAY:
                    m_PauseButton.gameObject.SetActive(true);
                    m_AddCoinsButton.gameObject.SetActive(false);
					m_TurnsIndicator.transform.parent.gameObject.SetActive (true);
					m_PointsIndicator.gameObject.SetActive (true);
                    break;

				case Systems.GameState.State.GAME_OVER:
				case Systems.GameState.State.PAUSE:
				case Systems.GameState.State.REWARDING:
                    m_PauseButton.gameObject.SetActive(false);
                    m_AddCoinsButton.gameObject.SetActive(true);
					m_TurnsIndicator.transform.parent.gameObject.SetActive (false);
					m_PointsIndicator.gameObject.SetActive (false);
                    break;
            }
        }

		private void SetWindowsState(Systems.GameState.State currentState)
        {
            switch (currentState)
            {
				case Systems.GameState.State.PLAY:

					break;

				case Systems.GameState.State.GAME_OVER:
					m_GameOverWindow.SetActive(true);
					Systems.GameState.Instance.UpdateBestScore();
                    break;

				case Systems.GameState.State.PAUSE:
					Systems.GameState.Instance.UpdateBestScore();
                    m_PauseWindow.SetActive(true);
                    break;

				case Systems.GameState.State.REWARDING:
					m_ClaimPrizeWindow.SetActive(true);
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

		private void UpdatePoints(int count)
		{
			m_PointsIndicator.text = count.ToString();
		}

		private void UpdateBestScore(int count)
		{
			m_BestScoreIndicator.text = count.ToString();
		}
    }
}
