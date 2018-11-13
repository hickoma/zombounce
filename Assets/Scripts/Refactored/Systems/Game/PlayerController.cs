using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using LeopotamGroup.Math;
using UnityEngine;

namespace Systems.PlayerProcessings
{
    [EcsInject]
	public class PlayerController : MonoBehaviour//, IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;

//        private EcsFilter<SetSprite> _setSpriteEventFilter = null;
//        private EcsFilter<PointerUpDownEvent> _pointerUpDownEventFilter = null;
//        private EcsFilter<GameStateEvent> _gameStateEventFilter = null;

		[SerializeField]
		private Player m_Player = null;

        private float _startPosition;
        private float _maxForceSqrt;
        private float _sqrtMinLength;

        private bool _isInteractive = true;


        public float MinLength
        {
            set { _sqrtMinLength = value * value; }
        }

        public float Multiplier;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

        public Sprite DeathSprite;
        public Sprite AliveSprite;

		public void LateStart()
		{
			_startPosition = m_Player.Transform.position.z;

			GameEventsController.Instance.OnSetPlayerSprite += SetPlayerSprite;
			GameEventsController.Instance.OnPointerUpDown += CheckInput;
			GameEventsController.Instance.OnGameStateChanged += CheckState;
		}

        public void Initialize()
        {
			
        }

        public void Destroy()
        {

        }

		public void Update()// Run()
        {
            CheckDistance();
        }

		private void CheckInput(bool isDown, Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
            if (_isInteractive)
            {
                if (isDown)
                {
                    m_Player.Rigidbody.velocity = Vector3.zero;

					DrawVector(downPointerPosition, upPointerPosition);
                }
				else
				{
					CalcAndSetForceVector(downPointerPosition, upPointerPosition);
				}
            }
        }

		private void DrawVector(Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
			Vector3 originalPosition = Camera.main.ScreenToWorldPoint(downPointerPosition);
			Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(upPointerPosition);

            CreateDrawEntity(originalPosition, (originalPosition - draggedPosition) * Multiplier, false);
        }

		private void CalcAndSetForceVector(Vector3 downPointerPosition, Vector3 upPointerPosition)
        {
			Vector3 originalPosition = Camera.main.ScreenToWorldPoint(downPointerPosition);
			Vector3 draggedPosition = Camera.main.ScreenToWorldPoint(upPointerPosition);

			Vector3 forceVector = (originalPosition - draggedPosition) * Multiplier;

            float sqrMagnitude = forceVector.sqrMagnitude;

            if (sqrMagnitude > _maxForceSqrt)
            {
                forceVector *= Mathf.Sqrt(_maxForceSqrt / sqrMagnitude);
            }

            if (forceVector.sqrMagnitude > _sqrtMinLength)
            {
                CreateForceEntity(forceVector);
                CreateCounterChangeEvent();
            }

            CreateDrawEntity(originalPosition, Vector3.zero, true);
        }

        private void CheckDistance()
        {
			if (_world == null)
			{
				_world = EcsWorld.Active;
			}

			var distance = _world.CreateEntityWith<DistanceEvent>();
            distance.CurrentDistance = m_Player.Transform.position.z - _startPosition;
        }

        private void SetPlayerSprite(bool isAlive)
        {
            if (isAlive)
            {
                m_Player.SpriteRenderer.sprite = AliveSprite;
            }
            else
            {
				m_Player.SpriteRenderer.sprite = DeathSprite;
            }
        }

        private void CreateForceEntity(Vector3 forceVector)
        {
			if (_world == null)
			{
				_world = EcsWorld.Active;
			}

            var forceEvent = _world.CreateEntityWith<AddForceEvent>();
            forceEvent.ForceVector = forceVector;
        }

        private void CreateCounterChangeEvent()
        {
			if (_world == null)
			{
				_world = EcsWorld.Active;
			}

            var component = _world.CreateEntityWith<TurnChangedEvent>();
            component.Changed = -1;
        }

        private void CreateDrawEntity(Vector3 downVector, Vector3 forceVector, bool release)
        {
			GameEventsController.Instance.DrawVectorPointer (downVector, forceVector, release);
        }

		private void CheckState(GameState newState)
        {
			switch (newState)
            {
                case GameState.PAUSE:
                case GameState.GAME_OVER:
                    _isInteractive = false;
                    CreateDrawEntity(Vector3.zero, Vector3.zero, true);
                    break;
                case GameState.PLAY:
                    _isInteractive = true;
                    break;
            }
        }
    }
}
