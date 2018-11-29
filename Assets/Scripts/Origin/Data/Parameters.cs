using UnityEngine;
using Components;

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

		public int CoinsCount;
        public int TurnsCount;
        public int TimerCount;
        public float RescaleSpeed;

        [Header("Camera"), Range(0f, 1f)] public float CameraSmooth;
        public float CameraMinPositionZ;

		public float BackBlockerDistanceFromCamera;

        [Header("Field Prefabs")] public GameObject[] Fields;
        public int ForwardSpawnCount;
		public int BackwardSpawnCount;
        public int InitialPoolSize;

        public GameObject Energy;
        public GameObject Coin;
        public int EnergySpawnCount;
        public int CoinSpawnCount;

        [Header("Fist Prefabs")] public Fist[] Fists;
		public Fist DefaultFist;
    }
}
