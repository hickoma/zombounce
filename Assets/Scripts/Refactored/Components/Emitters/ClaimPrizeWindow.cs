using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Windows
{
	public class ClaimPrizeWindow : MonoBehaviour
	{
		[SerializeField]
		private Button m_TakeCoinsButton = null;

		[SerializeField]
		private Text m_TakeCoinsText = null;

		[SerializeField]
		private Button m_TakeX3CoinsButton = null;

		[SerializeField]
		private Text m_TakeX3CoinsText = null;

		private int m_PrizeCoins = 0;
		private int m_PrizeCoinsX3 = 0;

		public void Start()
		{			
			m_TakeCoinsButton.onClick.AddListener(TakeCoins);
			m_TakeX3CoinsButton.onClick.AddListener(TakeX3Coins);
		}

		void OnEnable()
		{
			CountPrizes ();

			m_TakeCoinsText.text = m_PrizeCoins.ToString ();
			m_TakeX3CoinsText.text = m_PrizeCoinsX3.ToString ();
		}

		private void CountPrizes()
		{
			m_PrizeCoins = Mathf.CeilToInt ((float)Systems.GameState.Instance.CurrentPointsCount / Systems.GameState.Instance.PointsToCoinsCoeff);
			m_PrizeCoinsX3 = m_PrizeCoins * 3;
		}

		private void TakeCoins()
		{
			Systems.GameState.Instance.CoinsCount += m_PrizeCoins;
			RestartGame ();
		}

		private void TakeX3Coins()
		{
            ShowAds (() =>
            {
                Systems.GameState.Instance.CoinsCount += m_PrizeCoinsX3;
                RestartGame ();
            });
		}

		private void ShowAds(System.Action onAdvertisingClose)
		{
            GameEventsController.Instance.ShowAdvertising(onAdvertisingClose);
		}

		private void RestartGame()
		{
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
