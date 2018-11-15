using Components;
using UnityEngine;

namespace Systems.Physic
{
	public class AddForceController : MonoBehaviour
    {
		[SerializeField]
		private Player m_Player = null;

		public void LateStart()
		{
			GameEventsController.Instance.OnForceAdded += AddForce;
		}

		private void AddForce(Vector3 forceVector)
        {
            m_Player.m_Rigidbody.AddForce(forceVector);
        }
    }
}
