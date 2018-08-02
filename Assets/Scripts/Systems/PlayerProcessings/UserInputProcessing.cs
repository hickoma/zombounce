using Components;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.PlayerProcessings
{
    [EcsInject]
    public class UserInputProcessing : IEcsRunSystem
    {
        private EcsWorld _world = null;

        private bool _pressed;
        private Vector3 _downPointerPosition;

        public void Run()
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