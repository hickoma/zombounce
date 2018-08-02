using Components;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Game
{
    [EcsInject]
    public class CameraFollowProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;

        private EcsFilter<MainCamera> _cameraFilter = null;
        private EcsFilter<Player> _playerFilter = null;

        private MainCamera _camera;

        private Vector3 _velocity;

        public float CameraSmooth;

        public void Initialize()
        {
            foreach (var unityObject in GameObject.FindGameObjectsWithTag(Tag.Camera))
            {
                var tr = unityObject.transform;
                _camera = _world.CreateEntityWith<MainCamera>();
                _camera.Transform = tr;
            }
        }

        public void Destroy()
        {
            _camera.Transform = null;
            _camera = null;
        }

        public void Run()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            if (_cameraFilter.EntitiesCount == 0 || _playerFilter.EntitiesCount == 0) return;
            var camera = _cameraFilter.Components1[0];
            var player = _playerFilter.Components1[0];
            var currentPosition = camera.Transform.position;
            var smoothZ = Mathf.SmoothDamp(currentPosition.z, player.Transform.position.z, ref _velocity.z,
                CameraSmooth);
            var newPosition = new Vector3(currentPosition.x, currentPosition.y, smoothZ);
            camera.Transform.position = newPosition;
        }
    }
}