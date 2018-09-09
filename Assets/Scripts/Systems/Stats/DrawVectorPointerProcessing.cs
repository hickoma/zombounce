using Components;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using LeopotamGroup.Math;
using UnityEngine;

namespace Systems.Stats
{
    [EcsInject]
    public class DrawVectorPointerProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;

        private EcsFilter<DrawVectorPointer> _drawVectorPointerFilter = null;

        private VectorPointer _pointer;
        private float _maxScale;

        private float _maxForceSqrt;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

        public void Initialize()
        {
            var parent = GameObject.FindGameObjectWithTag(Tag.VectorPointer);
            var mainTr = parent.transform;
            var maskTr = mainTr.FindRecursiveByTag(Tag.VectorPointerMask);

            _pointer = _world.CreateEntityWith<VectorPointer>();
            _pointer.MainTransformForRotation = mainTr;
            _pointer.SpriteMaskTransform = maskTr;

            _maxScale = maskTr.localScale.y;
        }

        public void Destroy()
        {
            if (_pointer != null)
            {
                _pointer.MainTransformForRotation = null;
                _pointer.SpriteMaskTransform = null;
            }
        }

        public void Run()
        {
            for (int i = 0; i < _drawVectorPointerFilter.EntitiesCount; i++)
            {
                var entity = _drawVectorPointerFilter.Entities[i];

                var drawVectorPointerComponent = _drawVectorPointerFilter.Components1[i];
                var forceVector = drawVectorPointerComponent.ForceVector;

                var normalizedLenght = MathFast.Clamp01(Mathf.Sqrt(forceVector.sqrMagnitude / _maxForceSqrt));
                var lookPosition = forceVector.normalized;

                DrawVector(normalizedLenght, lookPosition);

                _world.RemoveEntity(entity);
            }
        }

        private void DrawVector(float normalizedLenght, Vector3 lookPosition)
        {
            var scaleSizeMultiplicator = 1f - normalizedLenght;
            var newYScale = _maxScale * scaleSizeMultiplicator;

            var v = _pointer.SpriteMaskTransform.localScale;
            _pointer.SpriteMaskTransform.localScale = new Vector3(v.x, newYScale, v.z);

            var angle = Vector3.SignedAngle(Vector3.forward, lookPosition, Vector3.down);
            _pointer.MainTransformForRotation.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}