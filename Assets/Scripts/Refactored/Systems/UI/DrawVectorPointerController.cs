using Components;
using UnityEngine;

namespace Systems.Ui
{
	public class DrawVectorPointerController : MonoBehaviour
    {
		[SerializeField]
		private VectorPointer m_VectorPointer = null;

        private float _maxScale;

        private float _maxForceSqrt;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

		public void LateStart()
		{
			GameEventsController.Instance.OnDrawVectorPointer += OnDrawVectorPointer;

			_maxScale = m_VectorPointer.SpriteMaskTransform.localScale.y;
		}

		public void OnDrawVectorPointer(Vector3 downVector, Vector3 forceVector, bool release)
		{
			downVector.y = 0f;

			float normalizedLenght = Mathf.Clamp01(Mathf.Sqrt(forceVector.sqrMagnitude / _maxForceSqrt));
			Vector3 lookPosition = forceVector.normalized;

			DrawVector(normalizedLenght, lookPosition);
		}

        private void DrawVector(float normalizedLenght, Vector3 lookPosition)
        {
            float scaleSizeMultiplicator = 1f - normalizedLenght;
            float newYScale = _maxScale * scaleSizeMultiplicator;

			Vector3 v = m_VectorPointer.SpriteMaskTransform.localScale;
			m_VectorPointer.SpriteMaskTransform.localScale = new Vector3(v.x, newYScale, v.z);

            float angle = Vector3.SignedAngle(Vector3.forward, lookPosition, Vector3.down);
			m_VectorPointer.MainTransformForRotation.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
