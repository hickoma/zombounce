using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    [EcsInject]
    public class UiProcessing: IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<OnRestartClickEvent> _turnDecrementEventFilter = null;

        public void Run()
        {
            if (_turnDecrementEventFilter.EntitiesCount > 0)
            {
                Restart();
            }
        }

        private void Restart()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }
    }
}