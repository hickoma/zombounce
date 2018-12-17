using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Emitters
{
    public class GroundEnterEmitter : MonoBehaviour
    {
		private Transform m_Transform;

        private void Awake()
        {
            m_Transform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.Player))
            {
				GameEventsController.Instance.EnterField (m_Transform.position.z);
            }
        }
    }
}
