using Components;
using Components.Events;
using LeopotamGroup.Ecs;

namespace Systems.Physic
{
    [EcsInject]
    public class AddForceProcessing : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<Player> _playerFilter;
        private EcsFilter<AddForeEvent> _addForceEventFilter;
        
        public void Run()
        {
            if (_addForceEventFilter.EntitiesCount > 0)
            {
                var forceVector = _addForceEventFilter.Components1[0].ForceVector;
                for (int i = 0; i < _playerFilter.EntitiesCount; i++)
                {
                    _playerFilter.Components1[i].Rigidbody.AddForce(forceVector);
                }

                _world.RemoveEntity(_addForceEventFilter.Entities[0]);
            }
        }
    }
}