using System;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class AdvertisingWindow : MonoBehaviour
    {
        [SerializeField]
        private Button m_CloseButton = null;

        private Action OnWindowClosed;

        void Start()
        {
            m_CloseButton.onClick.AddListener(HideWindow);
        }

        public void Init(Action onCloseAction)
        {
            OnWindowClosed = onCloseAction;
        }

        private void HideWindow()
        {
            gameObject.SetActive(false);

            if (OnWindowClosed != null)
            {
                OnWindowClosed();
            }
        }
    }
}
