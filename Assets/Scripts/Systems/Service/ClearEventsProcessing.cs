using Components.Events;
using LeopotamGroup.Ecs;

namespace Systems.Service
{
    [EcsInject]
    public class ClearEventsProcessing : IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PointerUpDownEvent> _pointerUpDownEventFilter = null;
        private EcsFilter<PlayerDeathEvent> _playerDeadEventFilter = null;
        private EcsFilter<TurnDecrementEvent> _turnDecrementEvent = null;
        private EcsFilter<OnRestartClickEvent> _onRestartClickEvent = null;

        public void Run()
        {
            for (int i = 0; i < _pointerUpDownEventFilter.EntitiesCount; i++)
            {
                _world.RemoveEntity(_pointerUpDownEventFilter.Entities[i]);
            }
            
            for (int i = 0; i < _playerDeadEventFilter.EntitiesCount; i++)
            {
                _world.RemoveEntity(_playerDeadEventFilter.Entities[i]);
            }
            
            for (int i = 0; i < _turnDecrementEvent.EntitiesCount; i++)
            {
                _world.RemoveEntity(_turnDecrementEvent.Entities[i]);
            }
            
            for (int i = 0; i < _onRestartClickEvent.EntitiesCount; i++)
            {
                _world.RemoveEntity(_onRestartClickEvent.Entities[i]);
            }
        }
    }
}