using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class GameOverWindow : MonoBehaviour
    {
		[SerializeField]
		private Button m_GetCoinsButton = null;

		[SerializeField]
		private Button m_RestartButton = null;

		public void Start()
		{			
			m_GetCoinsButton.onClick.AddListener(GameEventsController.Instance.StartGame);
			m_RestartButton.onClick.AddListener(GameEventsController.Instance.RestartGame);
		}
    }
}
