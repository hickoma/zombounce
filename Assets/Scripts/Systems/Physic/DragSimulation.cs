using Components;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Physic
{
    [EcsInject]
    public class DragSimulation : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter<Player> _playerFilter;

        private Vector3 _gravity;

        public float Drag;

        public void Initialize()
        {
            _gravity = Physics.gravity;
        }

        public void Destroy()
        {
        }

        public void Run()
        {
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                var player = _playerFilter.Components1[i];
                var rb = player.Rigidbody;

                Vector3 newVelocity = rb.velocity + _gravity * rb.mass * Time.fixedDeltaTime;
                newVelocity = newVelocity * Mathf.Clamp01(1f - Drag * Time.deltaTime);
                rb.velocity = newVelocity;
            }
        }
    }
}