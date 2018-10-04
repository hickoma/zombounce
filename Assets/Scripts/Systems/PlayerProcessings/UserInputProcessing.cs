using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.PlayerProcessings
{
    [EcsInject]
    public class UserInputProcessing : IEcsRunSystem
    {
        private EcsWorld _world = null;

        private EcsFilter<GameStateEvent> _gameStateEventFilter = null;

        private bool _pressed;
        private Vector3 _downPointerPosition;

        private bool _isInteractive = true;

        public void Run()
        {
            if (CheckInteractive())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _pressed = true;
                    _downPointerPosition = Input.mousePosition;
                }
                else if (_pressed && Input.GetMouseButtonUp(0))
                {
                    _pressed = false;
                    CreateUpEvent();
                }

                if (_pressed && Input.GetMouseButton(0))
                {
                    CreateDownEvent();
                }
            }
            else
            {
                _pressed = false;
            }
        }

        private bool CheckInteractive()
        {
            for (int i = 0; i < _gameStateEventFilter.EntitiesCount; i++)
            {
                switch (_gameStateEventFilter.Components1[i].State)
                {
                    case GameState.PAUSE:
                        _isInteractive = false;
                        break;
                    case GameState.PLAY:
                        _isInteractive = true;
                        break;
                    case GameState.NOT_INTERACTIVE:
                        _isInteractive = false;
                        break;
                }
            }

            return _isInteractive;
        }

        private void CreateUpEvent()
        {
            var upEvent = _world.CreateEntityWith<PointerUpDownEvent>();
            upEvent.isDown = false;
            upEvent.UpPointerPosition = Input.mousePosition;
        }

        private void CreateDownEvent()
        {
            var downEvent = _world.CreateEntityWith<PointerUpDownEvent>();
            downEvent.isDown = true;
            downEvent.DownPointerPosition = _downPointerPosition;
            downEvent.UpPointerPosition = Input.mousePosition;
        }
    }
}