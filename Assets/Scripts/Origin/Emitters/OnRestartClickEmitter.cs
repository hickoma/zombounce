using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Emitters
{
    public class OnRestartClickEmitter : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var world = EcsWorld.Active;
            if (world != null)
            {
                world.CreateEntityWith<OnRestartClickEvent>();
            }
        }
    }
}