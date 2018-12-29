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
				int currentTurns = Systems.GameState.Instance.TurnsCount;
				int lostTurns = 0;

				if (currentTurns > 1)
				{					
					// give player only one chance
					Systems.GameState.Instance.TurnsCount = 1;
					lostTurns = currentTurns - Systems.GameState.Instance.TurnsCount;
				}
				else
				{
					// kill player
					Systems.GameState.Instance.TurnsCount = -1;

					if (currentTurns == 1)
					{
						lostTurns = 1;
					}						
				}

				if (lostTurns > 0)
				{
					GameEventsController.Instance.ShowHitAnimation (lostTurns);
				}
            }
        }
    }
}