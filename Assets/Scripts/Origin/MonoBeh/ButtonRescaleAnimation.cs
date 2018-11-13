using UnityEngine;

namespace MonoBeh
{
    public class ButtonRescaleAnimation : MonoBehaviour
    {
        [SerializeField] private float _amplitude = 0.03f;
        [SerializeField] private float _rate = 5f;
        private Vector3 _scale;

        void Update()
        {
            for (int i = 0; i < 3; ++i)
            {
                _scale[i] = _amplitude * Mathf.Sin(Time.unscaledTime * _rate) + 1;
                transform.localScale = _scale;
            }
        }
    }
}