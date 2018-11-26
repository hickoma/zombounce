using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private Button m_HomeButton = null;

        private List<Windows.FistItemView> m_Items = new List<FistItemView>();
        private List<string> m_PurchasedFists = new List<string>();
        private List<Components.Fist> m_AllFists = new List<Components.Fist>();
        private string m_SelectedFist = "";
		private int m_CurrentCoins = 0;

        public Components.Fist[] AllFists
        {
            set
            {
                m_AllFists = new System.Collections.Generic.List<Components.Fist>(value);
            }
        }

        public void LateStart()
        {
            Init();

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
			// get coins count from Player Prefs
			m_CurrentCoins = PlayerPrefs.GetInt(Data.PrefKeys.CoinsKey, 500);

            // load purchased fists and use them to set right values in m_AllFists
            if (PlayerPrefs.HasKey(Data.PrefKeys.FistsKey))
            {
                m_PurchasedFists = new List<string>(PlayerPrefsExtensions.GetStringArray(Data.PrefKeys.FistsKey));
            }

            // load selected fist
            if (PlayerPrefs.HasKey(Data.PrefKeys.SelectedFistKey))
            {
                m_SelectedFist = PlayerPrefs.GetString(Data.PrefKeys.SelectedFistKey);
            }

            foreach(Components.Fist fist in m_AllFists)
            {
                // create fist item view by model
                Windows.FistItemView item = CreateFistItem(fist);
                m_Items.Add(item);

				// select current fist
				if (m_SelectedFist == fist.m_Id)
				{
					item.Select();
				}
            }

            // move More Content text to the end
            m_MoreContent.transform.SetAsLastSibling();

			UpdateCoinsCounter ();
        }

        private Windows.FistItemView CreateFistItem(Components.Fist fistModel)
        {
			bool isPurchased = false;

			if (m_PurchasedFists.Contains(fistModel.m_Id))
			{
				isPurchased = true;
			}

            Windows.FistItemView fistItem = Instantiate<Windows.FistItemView>(m_ItemPrefab, m_Content);
			fistItem.Init(fistModel, isPurchased, OnBuyItemClick, OnSelectItemClick);

            // select if needed
            if (fistModel.m_Id == Data.PrefKeys.SelectedFistKey)
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
				PlayerPrefs.SetInt(Data.PrefKeys.CoinsKey, m_CurrentCoins);
				UpdateCoinsCounter();

                // open new fist and save to Player Prefs
                fistView.m_Model.m_IsInInventory = true;
                m_PurchasedFists.Add(fistView.m_Model.m_Id);
				PlayerPrefsExtensions.SetStringArray (Data.PrefKeys.FistsKey, m_PurchasedFists.ToArray ());
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
			m_SelectedFist = fistView.m_Model.m_Id;
			PlayerPrefs.SetString(Data.PrefKeys.SelectedFistKey, m_SelectedFist);
			PlayerPrefs.Save();

            // notify player
            GameEventsController.Instance.SetFist(fistView.m_Model.m_Sprite.sprite);
        }

		private void UpdateCoinsCounter()
		{
			m_CoinsCount.text = m_CurrentCoins.ToString ();
		}
    }
}
