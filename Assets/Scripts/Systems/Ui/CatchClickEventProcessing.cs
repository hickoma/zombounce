using Components.Events;
using LeopotamGroup.Ecs;

namespace Systems.Ui
{
    [EcsInject]
    public class CatchClickEventProcessing : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<OnRestartClickEvent> _onRestartClickEvent = null;
        private EcsFilter<OnStartClickEvent> _onStartClickEvent = null;

        public void Run()
        {
            for (int i = 0; i < _onRestartClickEvent.EntitiesCount; i++)
            {
                _world.CreateEntityWith<RestartEvent>();
                _world.RemoveEntity(_onRestartClickEvent.Entities[i]);
            }
            
            for (int i = 0; i < _onStartClickEvent.EntitiesCount; i++)
            {
                _world.CreateEntityWith<GameStateEvent>().State = GameState.PLAY;
                _world.RemoveEntity(_onStartClickEvent.Entities[i]);
            }
        }
    }
}