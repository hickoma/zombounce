using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Emitters
{
    public class EnergyEmitter : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.Player))
            {
                var world = EcsWorld.Active;
                if (world != null)
                {
                    var entity = world.CreateEntityWith<TurnChangedEvent>();
                    entity.Changed = 1;
                    
                    var entity2 = world.CreateEntityWith<DespawnEnergyEvent>();
                    entity2.PoolObject = GetComponent<IPoolObject>();
                }
            }
        }
    }
}