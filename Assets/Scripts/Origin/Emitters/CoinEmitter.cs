using Components.Events;
using Data;
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
				Systems.GameState.Instance.CoinsCount += 1;
				GameEventsController.Instance.CreateRewardCoin (transform.position, 1);
				GameEventsController.Instance.GatherCoin (GetComponent<IPoolObject> ());
            }
        }
    }
}
