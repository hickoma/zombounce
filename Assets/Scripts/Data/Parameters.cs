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
		public int SecondLifeTurns;
		public int PointsToCoinsCoeff;
        public int AdvertisingCoinsMultiplierCoeff;
        public int FreeCoinsAmount;        
        public int LosesToShowInterstitialCount;

        [Header("Camera"), Range(0f, 1f)] public float CameraSmooth;
        public float CameraMinPositionZ;

		public float BackBlockerDistanceFromCamera;

		[Header("Difficulty Parameters")]
		public int NormalModePoints;
		public int HardModePoints;
		public int EnergySpawnEasyModeCount;
		public int EnergySpawnNormalModeCount;
		public int EnergySpawnHardModeCount;

        [Header("Levels")]
        public int ForwardSpawnCount;
		public int BackwardSpawnCount;
        public GameObject[] EasyLevels;
        public GameObject[] NormalLevels;
        public GameObject[] HardLevels;

		[Header("Zombie Prefabs")] public GameObject[] Zombies;
		public GameObject Coin;        
		public int CoinSpawnCount;

        [Header("Fist Prefabs")] public Fist[] Fists;
		public Fist DefaultFist;

		[Header("Tutorial")]
		public int TutorialShowTimes;
		public float FirstPartLength;
		public float SecondPartLength;
		public float ThirdPartLength;
		public GameObject[] FirstSessionLevels;

		[Header("Effects")]
		public float RescaleSpeed;
		public float RewardFlyLength;
		public float TurnEffectLength;
		public float TurnEffectDeltaY;

        [Header("Plugins ids and keys")] public string IronSourceAppKey;
    }
}
