using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	public class TurnReward : HudReward
	{
        public override void GiveReward()
        {
            Systems.GameState.Instance.TurnsCount += 1;
			base.GiveReward ();
        }
	}
}
