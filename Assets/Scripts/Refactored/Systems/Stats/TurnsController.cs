using UnityEngine;

namespace Systems
{
    public class TurnsController : MonoBehaviour
    {
		public void LateStart()
		{
			Systems.GameState.Instance.OnTurnsChanged += ChangeTurns;
			GameEventsController.Instance.OnPlayerStopped += CheckDeath;
		}

		private void CheckDeath(float zPosition)
        {
			int currentCount = Systems.GameState.Instance.TurnsCount;

			// last turn and player stop - he's dead
			if (currentCount == 0 && !Systems.GameState.Instance.FlyingRewardsExist)
            {
				GameEventsController.Instance.ChangeGameState (Systems.GameState.State.GAME_OVER);
            }
        }

		private void ChangeTurns(int newCount)
		{
			// becomes negative after hitting Red Block Emitter
			if (newCount < 0)
			{
				GameEventsController.Instance.ChangeGameState (Systems.GameState.State.GAME_OVER);
			}
		}
    }
}
