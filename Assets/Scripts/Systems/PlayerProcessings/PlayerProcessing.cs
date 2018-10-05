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
        private EcsFilter<SetSprite> _setSpriteEventFilter = null;
        private EcsFilter<PointerUpDownEvent> _pointerUpDownEventFilter = null;
        private EcsFilter<GameStateEvent> _gameStateEventFilter = null;

        private Player _player;
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
            CheckChangeSpriteEvent();
            CheckInput();
            CheckDistance();
            CheckState();
        }


        private void CheckChangeSpriteEvent()
        {
            for (int i = 0; i < _setSpriteEventFilter.EntitiesCount; i++)
            {
                SetPlayerSprite(_setSpriteEventFilter.Components1[i].isLive);
                _world.RemoveEntity(_setSpriteEventFilter.Entities[i]);
            }
        }

        private void CheckInput()
        {
            if (_isInteractive)
            {
                for (var i = 0; i < _pointerUpDownEventFilter.EntitiesCount; i++)
                {
                    var upDown = _pointerUpDownEventFilter.Components1[i];
                    if (upDown.isDown)
                    {
                        for (int j = 0; j < _playerFilter.EntitiesCount; j++)
                        {
                            _playerFilter.Components1[i].Rigidbody.velocity = Vector3.zero;
                        }

                        DrawVector(upDown);
                        continue;
                    }

                    for (var j = 0; j < _playerFilter.EntitiesCount; j++)
                    {
                        CalcAndSetForceVector(upDown);
                    }
                }
            }
        }

        private void DrawVector(PointerUpDownEvent upDown)
        {
            var originalPosition = Camera.main.ScreenToWorldPoint(upDown.DownPointerPosition);
            var draggedPosition = Camera.main.ScreenToWorldPoint(upDown.UpPointerPosition);

            CreateDrawEntity(originalPosition, (originalPosition - draggedPosition) * Multiplier, false);
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

            if (forceVector.sqrMagnitude > _sqrtMinLength)
            {
                CreateFroceEntity(forceVector);
                CreateCounterChangeEvent();
            }

            CreateDrawEntity(originalPosition, Vector3.zero, true);
        }

        private void CheckDistance()
        {
            var distance = _world.CreateEntityWith<DistanceEvent>();
            distance.CurrentDistance = _player.Transform.position.z - _startPosition;
        }

        private void SetPlayerSprite(bool isAlive)
        {
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                var playerComponent = _playerFilter.Components1[i];
                if (isAlive)
                {
                    playerComponent.SpriteRenderer.sprite = AliveSprite;
                }
                else
                {
                    playerComponent.SpriteRenderer.sprite = DeathSprite;
                }
            }
        }

        private void CreateFroceEntity(Vector3 forceVector)
        {
            var forceEvent = _world.CreateEntityWith<AddForeEvent>();
            forceEvent.ForceVector = forceVector;
        }

        private void CreateCounterChangeEvent()
        {
            var component = _world.CreateEntityWith<TurnChangedEvent>();
            component.Changed = -1;
        }

        private void CreateDrawEntity(Vector3 downVector, Vector3 forceVector, bool release)
        {
            var drawVectorPointerComponent = _world.CreateEntityWith<DrawVectorPointerEvent>();
            drawVectorPointerComponent.DownVector = downVector;
            drawVectorPointerComponent.ForceVector = forceVector;
            drawVectorPointerComponent.Release = release;
        }

        private void CheckState()
        {
            for (int i = 0; i < _gameStateEventFilter.EntitiesCount; i++)
            {
                switch (_gameStateEventFilter.Components1[i].State)
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
}