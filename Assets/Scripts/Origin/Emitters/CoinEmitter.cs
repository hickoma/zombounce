using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace Emitters
{
    public class CoinEmitter : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.Player))
            {
                var world = EcsWorld.Active;
                if (world != null)
                {
                    var entity = world.CreateEntityWith<CoinsChangedEvent>();
                    entity.Changed = 1;
                    
                    var entity2 = world.CreateEntityWith<DespawnCoinEvent>();
                    entity2.PoolObject = GetComponent<IPoolObject>();
                }
            }
        }
    }
}