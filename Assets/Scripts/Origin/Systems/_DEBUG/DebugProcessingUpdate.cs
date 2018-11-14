using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems._DEBUG
{
    [EcsInject]
    public class DebugProcessingUpdate : IEcsRunSystem
    {
		private Player m_Player;
        
        public void Run()
        {
//            DrawPath();
        }
        
        //debug

        private float delay = 0f;
        private float newtTime = 0f;
        private void DrawPath()
        {
            if (Time.time > newtTime)
            {
				if (m_Player == null)
				{
					m_Player = GameEventsController.Instance.m_Player;
				}

				newtTime = Time.time + delay;
				var cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				var scale = 0.6f;
				cube.transform.localScale = new Vector3 (scale, scale, scale);
				cube.transform.position = m_Player.m_Transform.position;
				cube.GetComponent<BoxCollider> ().enabled = false;
            }
        }
    }
}