using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Physic
{
    [EcsInject]
    public class DragSimulation : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
		private Player m_Player;

        private Vector3 _gravity;

        public float Drag;

        public void Initialize()
        {
            _gravity = Physics.gravity;
        }

        public void Destroy()
        {
        }

        public void Run()
        {
			if (m_Player == null)
			{
				m_Player = GameEventsController.Instance.m_Player;
			}

			var rb = m_Player.Rigidbody;

            Vector3 newVelocity = rb.velocity + _gravity * rb.mass * Time.fixedDeltaTime;
            newVelocity = newVelocity * Mathf.Clamp01(1f - Drag * Time.deltaTime);
            rb.velocity = newVelocity;
        }
    }
}