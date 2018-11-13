using Components.Events;
using LeopotamGroup.Ecs;

namespace Systems.Service
{
    [EcsInject]
    public class ClearEventsProcessing : IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PointerUpDownEvent> _pointerUpDownEventFilter = null;
        private EcsFilter<GameStateEvent> _gameStateEvent = null;

        public void Run()
        {
            for (int i = 0; i < _pointerUpDownEventFilter.EntitiesCount; i++)
            {
                _world.RemoveEntity(_pointerUpDownEventFilter.Entities[i]);
            }
            
            for (int i = 0; i < _gameStateEvent.EntitiesCount; i++)
            {
                _world.RemoveEntity(_gameStateEvent.Entities[i]);
            }
        }
    }
}