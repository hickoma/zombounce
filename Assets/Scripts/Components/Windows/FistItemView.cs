using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class FistItemView : MonoBehaviour
    {
        // model
        public Components.Fist m_Model = null;

        // view
        public Button m_SelectButton = null;
        public Image m_Sprite = null;
        public Image m_ChooseMark = null;
        public Image m_CheckMark = null;
        public Text m_Price = null;
        public Button m_BuyButton = null;

		public void Init(Components.Fist fistModel, bool isPurchased, System.Action<FistItemView> onBuy, System.Action<FistItemView> onSelect)
        {
			m_Model.Init(fistModel, isPurchased);

            // activities
			m_SelectButton.enabled = m_Model.m_IsInInventory;
			m_Sprite.sprite = m_Model.m_Sprite.sprite;

            m_ChooseMark.gameObject.SetActive(m_Model.m_IsInInventory);
            m_CheckMark.gameObject.SetActive(false);
			m_Price.text = m_Model.m_Price.ToString();
			m_BuyButton.gameObject.SetActive(!m_Model.m_IsInInventory);

            // button actions
            m_SelectButton.onClick.AddListener(() => onSelect(this));
            m_BuyButton.onClick.AddListener(() => onBuy(this));
        }

        public void Select()
        {
            m_ChooseMark.gameObject.SetActive(false);
            m_CheckMark.gameObject.SetActive(true);
        }

        public void Unselect()
        {
            m_ChooseMark.gameObject.SetActive(true);
            m_CheckMark.gameObject.SetActive(false);
        }

        public void Unlock()
        {
            m_ChooseMark.gameObject.SetActive(true);
            m_SelectButton.enabled = true;
            m_BuyButton.gameObject.SetActive(false);
        }
    }
}
