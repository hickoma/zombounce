using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Windows
{
    public class GameOverWindow : MonoBehaviour
    {
		[SerializeField]
		private Button m_PlayMoreButton = null;

		[SerializeField]
		private Button m_GetEnergyButton = null;

		[SerializeField]
		private Button m_GetCoinsButton = null;

		[SerializeField]
		private Button m_RestartButton = null;

		public void Start()
		{			
			m_PlayMoreButton.onClick.AddListener(PlayMore);
//			m_GetCoinsButton.onClick.AddListener(GameEventsController.Instance.StartGame);
			m_GetEnergyButton.onClick.AddListener(PlayMore);
//			m_RestartButton.onClick.AddListener(RestartGame);
		}

		private void PlayMore()
		{
			// refactor it
			LeopotamGroup.Ecs.EcsWorld world = LeopotamGroup.Ecs.EcsWorld.Active;
			if (world != null)
			{
				world.CreateEntityWith <Components.Events.PlayMoreEvent>().Energy = 5;
				world.CreateEntityWith<Components.Events.StartStopTimerEvent>().IsStart = false;
			}
		}

		private void RestartGame()
		{
			GameEventsController.Instance.RestartGame();

			SceneManager.LoadScene(0);
			// is it really needed?
			Time.timeScale = 1f;
		}
    }
}
