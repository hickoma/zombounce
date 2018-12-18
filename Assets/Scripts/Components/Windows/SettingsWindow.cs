using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class SettingsWindow : MonoBehaviour
    {
		[SerializeField]
		private Button m_HomeButton = null;

		public void LateStart()
		{
			m_HomeButton.onClick.AddListener(HideWindow);
			GameEventsController.Instance.OnSettingsClick += ShowWindow;
		}

		private void ShowWindow()
		{
			gameObject.SetActive(true);
		}

		private void HideWindow()
		{
			gameObject.SetActive(false);
		}
    }
}
