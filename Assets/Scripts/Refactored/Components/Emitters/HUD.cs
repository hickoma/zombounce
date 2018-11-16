using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class HUD : MonoBehaviour
    {
		[SerializeField]
		private Button m_PauseButton = null;

		[SerializeField]
		private Button m_SettingsButton = null;

		public void LateStart()
		{
			m_PauseButton.onClick.AddListener (GameEventsController.Instance.PauseGame);
//			m_SettingsButton.onClick.AddListener (GameEventsController.Instance.OpenSettings);
		}
    }
}
