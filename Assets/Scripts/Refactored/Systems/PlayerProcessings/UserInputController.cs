using Components.Events;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.PlayerProcessings
{
    public class UserInputController : MonoBehaviour
    {
		private bool m_Pressed;
		private Vector3 m_DownPointerPosition;
		private bool m_IsInteractive;

		public void LateStart()
		{
			GameEventsController.Instance.OnGameStateChanged += CheckInteractive;
		}

        public void Update()
        {
			if (m_IsInteractive)
            {
                if (CheckInterrupt())
                {
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    m_Pressed = true;
                    m_DownPointerPosition = Input.mousePosition;
                }
                else if (m_Pressed && Input.GetMouseButtonUp(0))
                {
                    m_Pressed = false;
                    CreateUpEvent();
                }

                if (m_Pressed && Input.GetMouseButton(0))
                {
                    CreateDownEvent();
                }
            }
        }

        private bool CheckInterrupt()
        {
			GameObject currentSelection = EventSystem.current.currentSelectedGameObject;
            return currentSelection != null && currentSelection.CompareTag(Tag.PauseButton);
        }

		private void CheckInteractive(Components.Events.GameState newState)
        {
			switch (newState)
            {
				case Components.Events.GameState.GAME_OVER:
                    m_IsInteractive = false;
                    m_Pressed = false;
                    break;
				case Components.Events.GameState.PAUSE:
                    m_IsInteractive = false;
                    break;
				case Components.Events.GameState.PLAY:
                    m_IsInteractive = true;
                    break;
            }
        }

        private void CreateUpEvent()
        {
			GameEventsController.Instance.PointerUpDown (false, m_DownPointerPosition, Input.mousePosition);
        }

        private void CreateDownEvent()
        {
			GameEventsController.Instance.PointerUpDown (true, m_DownPointerPosition, Input.mousePosition);
        }
    }
}
