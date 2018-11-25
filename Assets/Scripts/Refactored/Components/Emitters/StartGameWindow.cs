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

		public void Start()
		{
			m_StoreButton.onClick.AddListener(GameEventsController.Instance.OpenStore);
			m_BackgroundButton.onClick.AddListener(GameEventsController.Instance.StartGame);
		}
    }
}
