using Components;
using UnityEngine;

namespace Systems.Game
{
	public class CameraFollowController : MonoBehaviour
    {
		[SerializeField]
		private Player m_Player = null;

		[SerializeField]
		private MainCamera m_MainCamera = null;

        private Vector3 _velocity;

        public float CameraSmooth;
        public float CameraMinPositionZ;

        public void Update()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
			Vector3 currentPosition = m_MainCamera.m_Transform.position;
			float playerPositionZ = m_Player.m_Transform.position.z;
            float smoothZ = Mathf.SmoothDamp(currentPosition.z, playerPositionZ, ref _velocity.z,
                CameraSmooth);
            Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y, smoothZ);

			// don't move back
			if (newPosition.z > currentPosition.z)
			{
				// don't move behind game's beginning
				if (newPosition.z < CameraMinPositionZ)
				{
					newPosition.z = CameraMinPositionZ;
				}

				m_MainCamera.m_Transform.position = newPosition;
			}
        }
    }
}
