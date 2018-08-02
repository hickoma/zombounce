using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems._DEBUG
{
    [EcsInject]
    public class DebugProcessingFixedUpdate : IEcsRunSystem
    {
        private EcsFilter<Player> _player;
        
        public void Run()
        {
//            DragSimulation();
        }
        
        //debug

        private Vector3 gForceVector = new Vector3(0f, 9.81f, 0f);
        private float myDrag = 1f;
        
        private void DragSimulation()
        {
                for (int i = 0; i < _player.EntitiesCount; i++)
                {
                    var player = _player.Components1[i];
                    var rb = player.Rigidbody;
                    
                    Vector3 newVelocity = rb.velocity + gForceVector * rb.mass * Time.deltaTime;
                    newVelocity = newVelocity * Mathf.Clamp01(1f - myDrag*Time.deltaTime);
                    rb.velocity = newVelocity; 
                }
        }
    }
}