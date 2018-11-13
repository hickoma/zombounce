using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Emitters
{
    public class OnPlayClickEmitter : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var world = EcsWorld.Active;
            if (world != null)
            {
                world.CreateEntityWith<OnStartClickEvent>();
            }
        }
    }
}