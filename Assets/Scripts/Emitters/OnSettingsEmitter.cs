using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Emitters
{
    public class OnSettingsEmitter : MonoBehaviour, IPointerClickHandler
    {
        public bool OpenSettings;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var world = EcsWorld.Active;
            if (world != null)
            {
                world.CreateEntityWith<OnSettingsClickEvent>().OpenSettings = OpenSettings;
            }
        }
    }
}