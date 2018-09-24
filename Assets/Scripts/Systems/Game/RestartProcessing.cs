using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    [EcsInject]
    public class RestartProcessing: IEcsRunSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<RestartEvent> _restartEvent = null;

        public void Run()
        {
            if (_restartEvent.EntitiesCount > 0)
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