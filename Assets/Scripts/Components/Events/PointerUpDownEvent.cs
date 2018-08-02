using UnityEngine;

namespace Components.Events
{
    public sealed class PointerUpDownEvent
    {
        public bool isDown;
        public Vector3 DownPointerPosition;
        public Vector3 UpPointerPosition;
    }
}
