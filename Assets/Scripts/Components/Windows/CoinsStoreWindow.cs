using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class CoinsStoreWindow : MonoBehaviour
    {
        [SerializeField]
        private Button m_FreeCoinsButton = null;

        [SerializeField]
        private Button m_HomeButton = null;

        public void Start()
        {
            m_HomeButton.onClick.AddListener(HideWindow);
            m_FreeCoinsButton.onClick.AddListener(TakeFreeCoins);
        }

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }

        private void TakeFreeCoins()
        {
            ShowAds (() =>
            {
                Systems.GameState.Instance.CoinsCount += Systems.GameState.Instance.FreeCoinsAmount;
            });
        }

        private void ShowAds(System.Action onAdvertisingClose)
        {
            GameEventsController.Instance.ShowRewardVideo(onAdvertisingClose);
        }
    }
}
