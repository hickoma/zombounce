using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using LeopotamGroup.Math;
using UnityEngine;

namespace Systems.PlayerProcessings
{
    [EcsInject]
    public class PlayerProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;

        private EcsFilter<Player> _playerFilter = null;
        private EcsFilter<PointerUpDownEvent> _pointerUpDownEventFilter = null;
        private EcsFilter<PlayerDeathEvent> _deathEvent = null;

        private Player _player;
        private float _startPosition;
        private float _maxForceSqrt;

        public float Multiplier;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

        public Sprite DeathSprite;

        public void Initialize()
        {
            var unityObject = GameObject.FindGameObjectWithTag(Tag.Player);
            var tr = unityObject.transform;
            var rb = unityObject.GetComponent<Rigidbody>();
            var sr = unityObject.GetComponent<SpriteRenderer>();
            _player = _world.CreateEntityWith<Player>();
            _player.Transform = tr;
            _player.Rigidbody = rb;
            _player.SpriteRenderer = sr;

            _startPosition = tr.position.z;
        }

        public void Destroy()
        {
            _player.Transform = null;
            _player.Rigidbody = null;
            _player.SpriteRenderer = null;
            _player = null;
        }

        public void Run()
        {
            CheckDeathEvent();
            CheckInput();
            CheckDistance();
        }

        private void CheckDeathEvent()
        {
            if (_deathEvent.EntitiesCount > 0)
            {
                SetPlayerDeadSprite();
            }
        }

        private void CheckInput()
        {
            for (var i = 0; i < _pointerUpDownEventFilter.EntitiesCount; i++)
            {
                var upDown = _pointerUpDownEventFilter.Components1[i];
                if (upDown.isDown)
                {
                    DrawVector(upDown);
                    continue;
                }

                for (var j = 0; j < _playerFilter.EntitiesCount; j++)
                {
                    CalcAndSetForceVector(upDown);
                    var component = _world.CreateEntityWith<TurnChangedEvent>();
                    component.Changed = -1;
                }
            }
        }

        private void DrawVector(PointerUpDownEvent upDown)
        {
            var originalPosition = Camera.main.ScreenToWorldPoint(upDown.DownPointerPosition);
            var draggedPosition = Camera.main.ScreenToWorldPoint(upDown.UpPointerPosition);
            
            CreateDrawEntity((originalPosition - draggedPosition) * Multiplier);

        }

        private void CalcAndSetForceVector(PointerUpDownEvent upDown)
        {
            var originalPosition = Camera.main.ScreenToWorldPoint(upDown.DownPointerPosition);
            var draggedPosition = Camera.main.ScreenToWorldPoint(upDown.UpPointerPosition);

            var forceVector = (originalPosition - draggedPosition) * Multiplier;

            var sqrMagnitude = forceVector.sqrMagnitude;
            if (sqrMagnitude > _maxForceSqrt)
            {
                forceVector *= Mathf.Sqrt(_maxForceSqrt / sqrMagnitude);
            }

            CreateFroceEntity(forceVector);
            CreateDrawEntity(Vector3.zero);
        }

        private void CheckDistance()
        {
            var distance = _world.CreateEntityWith<DistanceEvent>();
            distance.CurrentDistance = _player.Transform.position.z - _startPosition;
        }

        private void SetPlayerDeadSprite()
        {
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                var playerComponent = _playerFilter.Components1[i];
                playerComponent.SpriteRenderer.sprite = DeathSprite;
            }
        }

        private void CreateFroceEntity(Vector3 forceVector)
        {
            var forceEvent = _world.CreateEntityWith<AddForeEvent>();
            forceEvent.ForceVector = forceVector;
        }
        
        private void CreateDrawEntity(Vector3 forceVector)
        {
            var drawVectorPointerComponent = _world.CreateEntityWith<DrawVectorPointer>();
            drawVectorPointerComponent.ForceVector = forceVector;
        }
    }
}