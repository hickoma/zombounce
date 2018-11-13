using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Service
{
    public class LevelInitializeProcessor: IEcsInitSystem
    {
        public void Initialize()
        {
            Time.timeScale = 1f;
        }

        public void Destroy()
        {
        }
    }
}