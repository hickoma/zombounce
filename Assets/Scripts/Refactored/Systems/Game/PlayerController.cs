using Components;
using Components.Events;
using UnityEngine;

namespace Systems.PlayerProcessings
{
	public class PlayerController : MonoBehaviour
    {
		[SerializeField]
		private Player m_Player = null;

		// parameters
        private float _startPosition;
        private float _maxForceSqrt;
        private float _sqrtMinLength;

        private bool _isInteractive = true;

        public float MinLength
        {
            set { _sqrtMinLength = value * value; }
        }

        public float Multiplier;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

        public Sprite DeathSprite;
        public Sprite AliveSprite;

		public void LateStart()
		{
			_startPosition = m_Player.m_Transform.position.z;

			GameEventsController.Instance.OnSetPlayerSprite += SetPlayerSprite;
			GameEventsController.Instance.OnPointerUpDown += CheckInput;
			GameEventsController.Instance.OnGameStateChanged += CheckState;
		}

		public void Update()
        {
            CheckDistance();
        }

		private void CheckInput(bool isDown, Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
            if (_isInteractive)
            {
                if (isDown)
                {
                    m_Player.m_Rigidbody.velocity = Vector3.zero;

					DrawVector(downPointerPosition, upPointerPosition);
                }
				else
				{
					CalcAndSetForceVector(downPointerPosition, upPointerPosition);
				}
            }
        }

		private void DrawVector(Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
			Vector3 originalPosition = Camera.main.ScreenToWorldPoint(downPointerPosition);
			Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(upPointerPosition);

            CreateDrawEntity(originalPosition, (originalPosition - draggedPosition) * Multiplier, false);
        }

		private void CalcAndSetForceVector(Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
			Vector3 originalPosition = Camera.main.ScreenToWorldPoint(downPointerPosition);
			Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(upPointerPosition);

			Vector3 forceVector = (originalPosition - draggedPosition) * Multiplier;

            float sqrMagnitude = forceVector.sqrMagnitude;

            if (sqrMagnitude > _maxForceSqrt)
            {
                forceVector *= Mathf.Sqrt(_maxForceSqrt / sqrMagnitude);
            }

            if (forceVector.sqrMagnitude > _sqrtMinLength)
            {
                CreateForceVector(forceVector);
                CreateCounterChangeEvent();
            }

            CreateDrawEntity(originalPosition, Vector3.zero, true);
        }

        private void CheckDistance()
        {
			float distance = m_Player.m_Transform.position.z - _startPosition;
			GameEventsController.Instance.ChangeDistance (distance);
        }

        private void SetPlayerSprite(bool isAlive)
        {
            if (isAlive)
            {
                m_Player.m_SpriteRenderer.sprite = AliveSprite;
            }
            else
            {
				m_Player.m_SpriteRenderer.sprite = DeathSprite;
            }
        }

        private void CreateForceVector(Vector3 forceVector)
        {
			GameEventsController.Instance.AddForce (forceVector);
        }

        private void CreateCounterChangeEvent()
        {
			GameEventsController.Instance.ChangeTurns (-1);
        }

        private void CreateDrawEntity(Vector3 downVector, Vector3 forceVector, bool release)
        {
			GameEventsController.Instance.DrawVectorPointer (downVector, forceVector, release);
        }

		private void CheckState(GameState newState)
        {
			switch (newState)
            {
                case GameState.PAUSE:
                case GameState.GAME_OVER:
                    _isInteractive = false;
                    CreateDrawEntity(Vector3.zero, Vector3.zero, true);
                    break;
                case GameState.PLAY:
                    _isInteractive = true;
                    break;
            }
        }
    }
}
