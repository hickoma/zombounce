using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class Fist : MonoBehaviour
    {
        public string m_Id = "fist_1";
        public int m_Price = 100;
        public SpriteRenderer m_Sprite;
        public bool m_IsInInventory = false;

		public void Init(Fist anotherFist, bool isPurchased)
		{
			m_Id = anotherFist.m_Id;
			m_Price = anotherFist.m_Price;
			m_Sprite = anotherFist.m_Sprite;
			m_IsInInventory = anotherFist.m_IsInInventory || isPurchased;
		}
    }
}
