using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class StartGameWindow : MonoBehaviour
    {
		[SerializeField]
        private Button m_StoreButton = null;

		[SerializeField]
		private Button m_BackgroundButton = null;

		// Debug buttons
		[SerializeField]
		private Button m_DropPurchases = null;

		public void Start()
		{
			m_StoreButton.onClick.AddListener(GameEventsController.Instance.OpenStore);
			m_BackgroundButton.onClick.AddListener(StartGame);
			m_DropPurchases.onClick.AddListener (GameEventsController.Instance.DropPurchases);
		}

		private void StartGame()
		{
			GameEventsController.Instance.StartGame();
			HideWindow();
		}

		private void HideWindow()
		{
			gameObject.SetActive(false);
		}
    }
}
