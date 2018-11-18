using Components;
using UnityEngine;

namespace Systems.Physic
{
	public class DragSimulationController : MonoBehaviour
    {
		[SerializeField]
		private Player m_Player = null;

        private Vector3 _gravity;

        public float Drag;

        public void LateStart()
        {
            _gravity = Physics.gravity;
        }

        public void FixedUpdate()
        {
			Rigidbody rb = m_Player.m_Rigidbody;

            Vector3 newVelocity = rb.velocity + _gravity * rb.mass * Time.fixedDeltaTime;
            newVelocity = newVelocity * Mathf.Clamp01(1f - Drag * Time.fixedDeltaTime);
            rb.velocity = newVelocity;
        }
    }
}
