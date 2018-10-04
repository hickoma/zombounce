using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Parameters", menuName = "Data")]
    public class Parameters : ScriptableObject
    {
        [Header("Player"), Range(100f, 1000f)] public float ForceMultiplier;
        public float Drag;
        public float MinLength;
        public float MinVelocityTolerance;
        
        [Range(1000f, 4000f)] public float MaxForce;

        public int TurnCount;
        public int TimerCount;
        public float RescaleSpeed;

        public Sprite AliveSprite;
        public Sprite DeadSprite;

        [Header("Camera"), Range(0f, 1f)] public float CameraSmooth;

        [Header("Field Prefabs")] public GameObject[] Fields;
        public int SpawnCount;
        public int InitialPoolSize;

        public GameObject Energy;
        public int EnergySpawnCount;
    }
}