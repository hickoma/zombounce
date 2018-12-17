using UnityEngine;

namespace MonoBeh
{
    public class ButtonRescaleAnimation : MonoBehaviour
    {
        [SerializeField] private float _amplitude = 0.03f;
        [SerializeField] private float _rate = 5f;

        void Update()
        {
            float newScale = _amplitude * Mathf.Sin(Time.unscaledTime * _rate) + 1;
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}