using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Components;

namespace Windows
{
    public class FistStoreWindow : MonoBehaviour
    {
		[SerializeField]
		private Text m_CoinsCount = null;

        [SerializeField]
        private Windows.FistItemView m_ItemPrefab = null;

        [SerializeField]
        private Transform m_Content = null;

        [SerializeField]
        private GameObject m_MoreContent = null;

        [SerializeField]
        private Button m_FreeCoinsButton = null;

        [SerializeField]
        private Button m_HomeButton = null;

        private List<Windows.FistItemView> m_Items = new List<FistItemView>();
        private List<string> m_PurchasedFists = new List<string>();
        private List<Fist> m_AllFists = new List<Fist>();
        private string m_SelectedFistName = "";
		private int m_CurrentCoins = 0;

        public void LateStart()
        {
            Init();

            m_FreeCoinsButton.onClick.AddListener(TakeFreeCoins);
            m_HomeButton.onClick.AddListener(HideWindow);
            GameEventsController.Instance.OnStoreWindowOpen += ShowWindow;
        }

        private void ShowWindow()
        {
            gameObject.SetActive(true);
        }

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }

        private void Init()
        {
			// load all fists from Game State
			m_AllFists = new List<Fist>(Systems.GameState.Instance.AllFists);

			// get coins count from Game State
			m_CurrentCoins = Systems.GameState.Instance.CoinsCount;

            // load purchased fists and use them to set right values in m_AllFists
			m_PurchasedFists = new List<string>(Systems.GameState.Instance.PurchasedFists);

            // load selected fist
			m_SelectedFistName = Systems.GameState.Instance.SelectedFistName;

            foreach(Fist fist in m_AllFists)
            {
                // create fist item view by model
                Windows.FistItemView item = CreateFistItem(fist);
                m_Items.Add(item);

				// select current fist
				if (m_SelectedFistName == fist.m_Id)
				{
					item.Select();
				}
            }

            // move More Content text to the end
            m_MoreContent.transform.SetAsLastSibling();

			UpdateCoinsCounter ();
        }

		private Windows.FistItemView CreateFistItem(Fist fistModel)
        {
			bool isPurchased = false;

			if (m_PurchasedFists.Contains(fistModel.m_Id))
			{
				isPurchased = true;
			}

            Windows.FistItemView fistItem = Instantiate<Windows.FistItemView>(m_ItemPrefab, m_Content);
			fistItem.Init(fistModel, isPurchased, OnBuyItemClick, OnSelectItemClick);

            // select if needed
			if (fistModel.m_Id == m_SelectedFistName)
            {
                OnSelectItemClick(fistItem);
            }

            return fistItem;
        }

        private void OnBuyItemClick(Windows.FistItemView fistView)
        {
			if (m_CurrentCoins > fistView.m_Model.m_Price)
            {
                // spend coins
				m_CurrentCoins -= fistView.m_Model.m_Price;
				Systems.GameState.Instance.CoinsCount = m_CurrentCoins;
				UpdateCoinsCounter();

                // open new fist and save to Player Prefs
                fistView.m_Model.m_IsInInventory = true;
                m_PurchasedFists.Add(fistView.m_Model.m_Id);
				Systems.GameState.Instance.PurchasedFists = m_PurchasedFists.ToArray ();
				PlayerPrefs.Save ();

                fistView.Unlock();
            }
        }

        private void OnSelectItemClick(Windows.FistItemView fistView)
        {
            fistView.Select();

            // unselect others
            foreach (var item in m_Items)
            {
                if (item != fistView)
                {
                    item.Unselect();
                }
            }

			// save selected fist to Player Prefs
			m_SelectedFistName = fistView.m_Model.m_Id;
			Systems.GameState.Instance.SelectedFistName = m_SelectedFistName;

            // notify player
            GameEventsController.Instance.SetFist(fistView.m_Model.m_Sprite.sprite);
        }

		private void UpdateCoinsCounter()
		{
			m_CoinsCount.text = m_CurrentCoins.ToString ();
		}

        private void TakeFreeCoins()
        {
            ShowAds (() =>
            {
                Systems.GameState.Instance.CoinsCount += Systems.GameState.Instance.FreeCoinsAmount;
                UpdateCoinsCounter();
            });
        }

        private void ShowAds(System.Action onAdvertisingClose)
        {
            GameEventsController.Instance.ShowRewardVideo(onAdvertisingClose);
        }
    }
}
