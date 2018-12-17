using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	public class CoinReward : HudReward
	{
		public override void GiveReward()
		{
			Systems.GameState.Instance.CoinsCount += 1;
			base.GiveReward ();
		}
	}
}
