using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Parameters", menuName = "Data")]
    public class Parameters : ScriptableObject
    {
        [Header("Player"), Range(100f, 1000f)] public float ForceMultiplier;
        public float Drag;

        public int TurnCount;

        public Sprite AliveSprite;
        public Sprite DeadSprite;

        [Header("Camera"), Range(0f, 1f)] public float CameraSmooth;

        [Header("Field Prefabs")] public GameObject[] Fields;
        public int SpawnCount;
        public int InitialPoolSize;
    }
}