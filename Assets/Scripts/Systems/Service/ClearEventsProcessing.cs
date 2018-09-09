using Components.Events;
using LeopotamGroup.Ecs;

namespace Systems.Service
{
    [EcsInject]
    public class ClearEventsProcessing : IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PointerUpDownEvent> _pointerUpDownEventFilter = null;
        private EcsFilter<PlayerDeathEvent> _playerDeathEventFilter = null;
        private EcsFilter<TurnChangedEvent> _turnChangedEvent = null;
        private EcsFilter<OnRestartClickEvent> _onRestartClickEvent = null;

        public void Run()
        {
            for (int i = 0; i < _pointerUpDownEventFilter.EntitiesCount; i++)
            {
                _world.RemoveEntity(_pointerUpDownEventFilter.Entities[i]);
            }
            
            for (int i = 0; i < _playerDeathEventFilter.EntitiesCount; i++)
            {
                _world.RemoveEntity(_playerDeathEventFilter.Entities[i]);
            }
            
            for (int i = 0; i < _turnChangedEvent.EntitiesCount; i++)
            {
                _world.RemoveEntity(_turnChangedEvent.Entities[i]);
            }
            
            for (int i = 0; i < _onRestartClickEvent.EntitiesCount; i++)
            {
                _world.RemoveEntity(_onRestartClickEvent.Entities[i]);
            }
        }
    }
}