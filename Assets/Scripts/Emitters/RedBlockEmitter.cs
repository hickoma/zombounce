using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Emitters
{
    public class RedBlockEmitter : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(Tag.Player))
            {
                var world = EcsWorld.Active;
                if (world != null)
                {
                    EcsWorld.Active.CreateEntityWith<PlayerDeathEvent>();
                }
            }
        }
    }
}