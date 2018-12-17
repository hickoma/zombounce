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

        [SerializeField]
        private Text m_TakeX3CoinsBadgeXText = null;

		private int m_PrizeCoins = 0;
		private int m_PrizeCoinsX3 = 0;
        private int m_AdvertisingMultiplier = 1;
        private Tween.Base m_X3CoinsButtonScaleTween = null;

		public void Start()
		{			
			m_TakeCoinsButton.onClick.AddListener(TakeCoins);
			m_TakeX3CoinsButton.onClick.AddListener(TakeX3Coins);
		}

		void OnEnable()
		{
			CountPrizes ();

            bool isRewardVideoAvailable = Systems.GameState.Instance.IsRewardVideoAvailable;

			m_TakeCoinsText.text = m_PrizeCoins.ToString ();
			m_TakeX3CoinsText.text = m_PrizeCoinsX3.ToString ();
            m_TakeX3CoinsBadgeXText.text = "x" + m_AdvertisingMultiplier.ToString();
            m_TakeX3CoinsButton.gameObject.SetActive(isRewardVideoAvailable);

            // show x3 button only if watching reward video is possible
            if (isRewardVideoAvailable)
            {
                AnimateX3CoinsButton();
            }
		}

        private void AnimateX3CoinsButton()
        {
            Transform getEnergyTransform = m_TakeX3CoinsButton.transform;
            m_X3CoinsButtonScaleTween = Tween.Value(1f).From(1f).To(1.3f).OnUpdate((v) =>
            {
                getEnergyTransform.localScale = Vector3.one * v;
            }).OnFinal(() =>
            {
                getEnergyTransform.localScale = Vector3.one;
            }).PingPong.Start();
        }

		private void CountPrizes()
		{
			m_PrizeCoins = Mathf.CeilToInt ((float)Systems.GameState.Instance.CurrentPointsCount / Systems.GameState.Instance.PointsToCoinsCoeff);
            m_AdvertisingMultiplier = Systems.GameState.Instance.AdvertisingCoinsMultiplierCoeff;
            m_PrizeCoinsX3 = m_PrizeCoins * m_AdvertisingMultiplier;
		}

		private void TakeCoins()
		{
			Systems.GameState.Instance.CoinsCount += m_PrizeCoins;
			RestartGame ();
		}

		private void TakeX3Coins()
		{
            ShowRewardVideo (() =>
            {
				// track reward taking
				Analytics.SendEventAnalytic (Analytics.PossibleEvents.CoinsX3Reward, Systems.GameState.Instance.SessionsCount.ToString());

                Systems.GameState.Instance.CoinsCount += m_PrizeCoinsX3;
                RestartGame ();
            });
		}

		private void ShowRewardVideo(System.Action onAdvertisingClose)
		{
            if (m_X3CoinsButtonScaleTween != null)
            {
                m_X3CoinsButtonScaleTween.Stop();
            }

            GameEventsController.Instance.ShowRewardVideo(onAdvertisingClose);
		}

		private void RestartGame()
		{
            if (m_X3CoinsButtonScaleTween != null)
            {
                m_X3CoinsButtonScaleTween.Stop();
            }

            ShowInterstitial(() =>
            {
                GameEventsController.Instance.RestartGame();

                SceneManager.LoadScene(0);
                HideWindow();
            });
		}

        private void ShowInterstitial(System.Action onInterstitialClose)
        {
            GameEventsController.Instance.ShowInterstitial(onInterstitialClose);
        }

		private void HideWindow()
		{
			gameObject.SetActive(false);
		}
	}
}
