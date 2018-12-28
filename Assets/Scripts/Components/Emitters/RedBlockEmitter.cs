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
				if (Systems.GameState.Instance.TurnsCount > 1)
				{					
					// give player only one chance
					Systems.GameState.Instance.TurnsCount = 1;
				}
				else
				{
					// kill player
					Systems.GameState.Instance.TurnsCount = -1;
				}
            }
        }
    }
}