﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Components;

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

//		[SerializeField]
//		private Text m_Banner = null;

        [Space]
        [Header("Windows")]
		[SerializeField]
		private GameObject m_StartGameWindow = null;

        [SerializeField]
        private GameObject m_PauseWindow = null;

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
		private TurnReward m_TurnRewardPrefab = null;

		[SerializeField]
		private CoinReward m_CoinRewardPrefab = null;

		[Space]
		[Header("Effects")]
		[SerializeField]
		private TurnEffect m_TurnEffect = null;

		[System.NonSerialized]
		public float m_TurnEffectLength;
		[System.NonSerialized]
		public float m_TurnEffectDeltaY;

		// special list to control Player death on last energy
		private List<TurnReward> m_FlyingTurnRewards = new List<TurnReward> ();

		public void LateStart()
		{
            GameEventsController.Instance.OnGameStateChanged += OnGameStateChanged;
//            GameEventsController.Instance.OnShowAdvertising += OnShowAdvertising;
			GameEventsController.Instance.OnCreateRewardTurn += OnCreateRewardTurn;
			GameEventsController.Instance.OnCreateRewardCoin += OnCreateRewardCoin;
            // debug stuff
//            GameEventsController.Instance.OnWriteToLog += (s) => m_Banner.text += "\n" + s;

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
			TurnReward turnReward = Instantiate (m_TurnRewardPrefab, transform) as TurnReward;
			// icon position relative to HUD
			Vector3 targetPosition = transform.InverseTransformPoint(m_TurnsIcon.rectTransform.position);
			turnReward.SetFlight (startPosition, targetPosition);

			// add to control list
			AddFlyingTurnReward (turnReward);
		}

		void AddFlyingTurnReward(TurnReward flyingReward)
		{
			m_FlyingTurnRewards.Add (flyingReward);
			flyingReward.OnFlightEnd += RemoveFlyingTurnReward;
			Systems.GameState.Instance.FlyingRewardsExist = (m_FlyingTurnRewards.Count > 0);
		}

		void RemoveFlyingTurnReward(HudReward flyingReward)
		{
			m_FlyingTurnRewards.Remove ((TurnReward)flyingReward);
			Systems.GameState.Instance.FlyingRewardsExist = (m_FlyingTurnRewards.Count > 0);
		}

		void OnCreateRewardCoin (Vector3 startPosition, int count)
		{
			CoinReward coinReward = Instantiate (m_CoinRewardPrefab, transform) as CoinReward;
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
			int currentTurnsCount = int.Parse (m_TurnsIndicator.text);

			// create effect
			if (currentTurnsCount > count)
			{
				TurnEffect turnEffect = Instantiate<TurnEffect> (m_TurnEffect, m_TurnsIndicator.transform.parent);
				turnEffect.m_AnimationLength = m_TurnEffectLength;
				turnEffect.m_AnimationDeltaY = m_TurnEffectDeltaY;
			}

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

        /*public void SendReport()
        {
            Application.OpenURL(FormErrorReport());
        }

		public string FormErrorReport()
		{
            string body = m_Banner.text;

			//отчёт в формате URL:
			return string.Format("mailto:{0}?subject={1}&body={2}",
				"vasilisk_91@mail.ru", //0
				"zombounce_error_log", //1
				ConvertToUrl(body)); //2
		}

        private static string ConvertToUrl(string value)
        {
            return WWW.EscapeURL(value).Replace("+", "%20");
        }*/
    }
}
