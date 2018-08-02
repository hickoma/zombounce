using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems._DEBUG
{
    [EcsInject]
    public class DebugProcessingUpdate : IEcsRunSystem
    {
        private EcsFilter<Player> _player;
        
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
                for (int i = 0; i < _player.EntitiesCount; i++)
                {
                    var player = _player.Components1[i];
                    
                    newtTime = Time.time + delay;
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var scale = 0.6f;
                    cube.transform.localScale = new Vector3(scale, scale, scale);
                    cube.transform.position = player.Transform.position;
                    cube.GetComponent<BoxCollider>().enabled = false;
                }
               
            }
        }
    }
}