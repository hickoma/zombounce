using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Emitters
{
    public class PlayMoreEmitter : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private int _energy = 5;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var world = EcsWorld.Active;
            if (world != null)
            {
                world.CreateEntityWith<PlayMoreEvent>().Energy = _energy;
                world.CreateEntityWith<StartStopTimerEvent>().IsStart = false;
            }
        }
    }
}