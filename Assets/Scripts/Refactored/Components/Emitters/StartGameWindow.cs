using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class StartGameWindow : MonoBehaviour
    {
		[SerializeField]
		private Button m_StartButton = null;

		[SerializeField]
		private Button m_BackgroundButton = null;

		public void Start()
		{
			m_StartButton.onClick.AddListener(GameEventsController.Instance.StartGame);
			m_BackgroundButton.onClick.AddListener(GameEventsController.Instance.StartGame);
		}
    }
}
