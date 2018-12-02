using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Windows
{
    public class PauseGameWindow : MonoBehaviour
    {
        [SerializeField]
        private Button m_ResumeButton = null;

        [SerializeField]
        private Button m_HomeButton = null;

        public void Start()
        {
            m_ResumeButton.onClick.AddListener(ResumeGame);
            m_HomeButton.onClick.AddListener(AbortGame);
        }

        private void ResumeGame()
        {
            GameEventsController.Instance.ResumeGame();
            HideWindow();
        }

        private void AbortGame()
        {
            GameEventsController.Instance.RestartGame();

            SceneManager.LoadScene(0);
            HideWindow();
        }

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
