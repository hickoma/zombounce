using Components;
using UnityEngine;

namespace Systems.Game
{
	public class BackBlockerFollowController : MonoBehaviour
	{
		[SerializeField]
		private Components.BackBlocker m_BackBlocker = null;

		[SerializeField]
		private Components.MainCamera m_MainCamera = null;

		public float DistanceFromCamera;

		public void Update()
		{
			MoveBlocker ();
		}

		void MoveBlocker()
		{
			Vector3 blockerCurrentPosition = m_BackBlocker.m_Transform.position;
			m_BackBlocker.m_Transform.position = new Vector3 (blockerCurrentPosition.x, blockerCurrentPosition.y, m_MainCamera.m_Transform.position.z + DistanceFromCamera);
		}
	}
}
