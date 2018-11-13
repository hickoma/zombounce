using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Emitters
{
    public class GroundEnterEmitter : MonoBehaviour
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.Player))
            {
                var world = EcsWorld.Active;
                if (world != null)
                {
                    var entity = world.CreateEntityWith<InFieldEvent>();
                    entity.ZPosition = _transform.position.z;
                }
            }
        }
    }
}