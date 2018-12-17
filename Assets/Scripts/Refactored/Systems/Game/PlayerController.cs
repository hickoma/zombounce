using Components;
using UnityEngine;
using System;
using System.Collections.Generic;

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

		public float MinVelocityTolerance;
        public float Multiplier;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

		private bool m_IsMoving = false;

		public void LateStart()
		{
			_startPosition = m_Player.m_Transform.position.z;

			Fist selectedFist = Systems.GameState.Instance.SelectedFist;

			if (selectedFist != null)
			{
				m_Player.m_FistSprite.sprite = selectedFist.m_Sprite.sprite;
			}

            GameEventsController.Instance.OnSetFist += SetFistSprite;
			GameEventsController.Instance.OnPointerUpDown += CheckInput;
			GameEventsController.Instance.OnGameStateChanged += CheckState;
		}

		public void Update()
        {
			CheckVelocity();
            CheckDistance();
            CheckDirection();
        }

		private void CheckInput(bool isDown, Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
            if (_isInteractive)
            {
                if (isDown)
                {
                    m_Player.m_Rigidbody.velocity = Vector3.zero;

					DrawVector(downPointerPosition, upPointerPosition);
                    Vector3 direction = upPointerPosition - downPointerPosition;

                    // to avoid strange rotations when force vector is near Vector3.zero
                    if (direction.sqrMagnitude > MinVelocityTolerance)
                    {
                        // fist follows the opposite direction of fingerprins
                        TurnFist(-direction.x, -direction.y);
                    }

                    m_Player.m_AnimatorController.SetBool("IsAiming", true);
                }
				else
				{
					CalcAndSetForceVector(downPointerPosition, upPointerPosition);
                    m_Player.m_AnimatorController.SetBool("IsAiming", false);
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
				Systems.GameState.Instance.TurnsCount -= 1;
            }

            CreateDrawEntity(originalPosition, Vector3.zero, true);
        }

		private void CheckVelocity()
		{
			if (!m_IsMoving && Math.Abs (m_Player.m_Rigidbody.velocity.sqrMagnitude) >
			    MinVelocityTolerance)
			{
				m_IsMoving = true;
			}

			if (m_IsMoving && Math.Abs (m_Player.m_Rigidbody.velocity.sqrMagnitude) <
				MinVelocityTolerance)
			{
				GameEventsController.Instance.PlayerStopped (m_Player.m_Transform.position.z);
				m_IsMoving = false;
			}
		}

        private void CheckDistance()
        {
			float distance = m_Player.m_Transform.position.z - _startPosition;
			GameEventsController.Instance.ChangeDistance (distance);
        }

        private void CheckDirection()
        {
            if (m_IsMoving)
            {
                TurnFist(m_Player.m_Rigidbody.velocity.x, m_Player.m_Rigidbody.velocity.z);
            }
        }

        private void TurnFist(float x, float z)
        {
            float currentAngle = m_Player.m_FistTransform.localRotation.eulerAngles.z;
            float newAngle = Mathf.Atan2(-x, z) * Mathf.Rad2Deg;
            m_Player.m_FistTransform.Rotate(Vector3.down, newAngle - currentAngle, Space.World);
        }

        private void SetFistSprite(Sprite sprite)
        {
            m_Player.m_FistSprite.sprite = sprite;
        }

        private void CreateForceVector(Vector3 forceVector)
        {
			GameEventsController.Instance.AddForce (forceVector);
        }

        private void CreateDrawEntity(Vector3 downVector, Vector3 forceVector, bool release)
        {
			GameEventsController.Instance.DrawVectorPointer (downVector, forceVector, release);
        }

		private void CheckState(GameState.State newState)
        {
			switch (newState)
            {
				case GameState.State.PAUSE:
				case GameState.State.GAME_OVER:
                    _isInteractive = false;
                    CreateDrawEntity(Vector3.zero, Vector3.zero, true);
                    break;
				case GameState.State.PLAY:
                    _isInteractive = true;
                    break;
            }
        }
    }
}
