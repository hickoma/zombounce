using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems._DEBUG
{
    [EcsInject]
    public class DebugProcessingFixedUpdate : IEcsRunSystem
    {
		private Player m_Player;
        
        public void Run()
        {
//            DragSimulation();
        }
        
        //debug

        private Vector3 gForceVector = new Vector3(0f, 9.81f, 0f);
        private float myDrag = 1f;
        
        private void DragSimulation()
        {
			if (m_Player == null)
			{
				m_Player = GameEventsController.Instance.m_Player;
			}

			var rb = m_Player.Rigidbody;
                
			Vector3 newVelocity = rb.velocity + gForceVector * rb.mass * Time.deltaTime;
			newVelocity = newVelocity * Mathf.Clamp01 (1f - myDrag * Time.deltaTime);
			rb.velocity = newVelocity; 
        }
    }
}