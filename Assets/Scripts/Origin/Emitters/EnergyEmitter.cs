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
				GameEventsController.Instance.CreateRewardTurn (transform.position, 1);
				GameEventsController.Instance.GatherEnergy (GetComponent<IPoolObject> ());
            }
        }
    }
}
