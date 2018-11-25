using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class FistStoreWindow : MonoBehaviour
    {
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
                if (m_PurchasedFists.Contains(fist.m_Id))
                {
                    fist.m_IsInInventory = true;
                }

                // create fist item view by model
                Windows.FistItemView item = CreateFistItem(fist);
                m_Items.Add(item);
            }

            // move More Content text to the end
            m_MoreContent.transform.SetAsLastSibling();

            // select first fist by default
            if (string.IsNullOrEmpty(m_SelectedFist) && m_Items.Count > 0)
            {
                m_Items[0].Select();
            }
        }

        private Windows.FistItemView CreateFistItem(Components.Fist fistModel)
        {
            Windows.FistItemView fistItem = Instantiate<Windows.FistItemView>(m_ItemPrefab, m_Content);
            fistItem.Init(fistModel, OnBuyItemClick, OnSelectItemClick);

            // select if needed
            if (fistModel.m_Id == Data.PrefKeys.SelectedFistKey)
            {
                OnSelectItemClick(fistItem);
            }

            return fistItem;
        }

        private void OnBuyItemClick(Windows.FistItemView fistView)
        {
            int currentCoins = PlayerPrefs.GetInt(Data.PrefKeys.CoinsKey, 500);

            if (currentCoins > fistView.m_Model.m_Price)
            {
                // spend coins
                currentCoins -= fistView.m_Model.m_Price;
                PlayerPrefs.SetInt(Data.PrefKeys.CoinsKey, currentCoins);

                // open new fist
                fistView.m_Model.m_IsInInventory = true;
                m_PurchasedFists.Add(fistView.m_Model.m_Id);
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

            // notify player
            GameEventsController.Instance.SetFist(fistView.m_Model.m_Sprite.sprite);
        }
    }
}
