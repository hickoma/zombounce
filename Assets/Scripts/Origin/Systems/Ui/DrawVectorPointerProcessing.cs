using Components;
using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using LeopotamGroup.Math;
using UnityEngine;

namespace Systems.Ui
{
    [EcsInject]
	public class DrawVectorPointerProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world = null;

        private EcsFilter<VectorPointer> _vectorPointerFilter = null;
        private EcsFilter<Fingerprint> _fingerprintrFilter = null;
        private EcsFilter<DrawVectorPointerEvent> _drawVectorPointerFilter = null;

        private float _maxScale;

        private float _maxForceSqrt;

        public float MaxForce
        {
            set { _maxForceSqrt = value * value; }
        }

//		public void LateStart()
//		{
//			GameEventsController.Instance.OnDrawVectorPointer += OnDrawVectorPointer;
//		}

        public void Initialize()
        {
			GameEventsController.Instance.OnDrawVectorPointer += OnDrawVectorPointer;
            InitPointerVector();
            InitFingerprint();
        }

        public void Destroy()
        {
            for (int i = 0; i < _vectorPointerFilter.EntitiesCount; i++)
            {
                _vectorPointerFilter.Components1[i].MainTransformForRotation = null;
                _vectorPointerFilter.Components1[i].SpriteMaskTransform = null;
            }

            for (int i = 0; i < _fingerprintrFilter.EntitiesCount; i++)
            {
                _fingerprintrFilter.Components1[i].Parent = null;
                _fingerprintrFilter.Components1[i].MainTransformForRotation = null;
                _fingerprintrFilter.Components1[i].SpriteMaskTransform = null;
            }
        }

        public void Run()
        {
//            for (int i = 0; i < _drawVectorPointerFilter.EntitiesCount; i++)
//            {
//                var entity = _drawVectorPointerFilter.Entities[i];
//
//                var drawVectorPointerComponent = _drawVectorPointerFilter.Components1[i];
//                var forceVector = drawVectorPointerComponent.ForceVector;
//                var downPointerVector = drawVectorPointerComponent.DownVector;
//                downPointerVector.y = 0f;
//
//                var normalizedLenght = MathFast.Clamp01(Mathf.Sqrt(forceVector.sqrMagnitude / _maxForceSqrt));
//                var lookPosition = forceVector.normalized;
//
//                DrawVector(normalizedLenght, lookPosition);
//                DrawFingerprint(downPointerVector, normalizedLenght, lookPosition,
//                    drawVectorPointerComponent.Release);
//
//                _world.RemoveEntity(entity);
//            }
        }

		public void OnDrawVectorPointer(Vector3 downVector, Vector3 forceVector, bool release)
		{
			downVector.y = 0f;

			float normalizedLenght = MathFast.Clamp01(Mathf.Sqrt(forceVector.sqrMagnitude / _maxForceSqrt));
			Vector3 lookPosition = forceVector.normalized;

			DrawVector(normalizedLenght, lookPosition);
			DrawFingerprint(downVector, normalizedLenght, lookPosition,
				release);
		}

        private void DrawVector(float normalizedLenght, Vector3 lookPosition)
        {
            for (int i = 0; i < _vectorPointerFilter.EntitiesCount; i++)
            {
                var pointer = _vectorPointerFilter.Components1[i];

                var scaleSizeMultiplicator = 1f - normalizedLenght;
                var newYScale = _maxScale * scaleSizeMultiplicator;

                var v = pointer.SpriteMaskTransform.localScale;
                pointer.SpriteMaskTransform.localScale = new Vector3(v.x, newYScale, v.z);

                var angle = Vector3.SignedAngle(Vector3.forward, lookPosition, Vector3.down);
                pointer.MainTransformForRotation.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void DrawFingerprint(Vector3 originalPosition, float normalizedLenght, Vector3 lookPosition,
            bool release)
        {
            for (int i = 0; i < _fingerprintrFilter.EntitiesCount; i++)
            {
                var fingerprintEvent = _fingerprintrFilter.Components1[i];
                var parent = fingerprintEvent.Parent;

                if (release)
                {
                    parent.SetActive(false);
                }
                else
                {
                    parent.transform.position = originalPosition;
                    parent.SetActive(true);

                    var scaleSizeMultiplicator = 1f - normalizedLenght;
                    var newYScale = _maxScale * scaleSizeMultiplicator;

                    var v = fingerprintEvent.SpriteMaskTransform.localScale;
                    fingerprintEvent.SpriteMaskTransform.localScale = new Vector3(v.x, newYScale, v.z);

                    var angle = 180f - Vector3.SignedAngle(Vector3.forward, lookPosition, Vector3.up);
                    fingerprintEvent.MainTransformForRotation.localRotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
        }

        private void InitPointerVector()
        {
            var parent = GameObject.FindGameObjectWithTag(Tag.VectorPointer);
            var mainTr = parent.transform;
            var maskTr = mainTr.FindRecursiveByTag(Tag.VectorPointerMask);

            var pointer = _world.CreateEntityWith<VectorPointer>();
            pointer.MainTransformForRotation = mainTr;
            pointer.SpriteMaskTransform = maskTr;

            _maxScale = maskTr.localScale.y;
        }

        private void InitFingerprint()
        {
            var fingerprint = GameObject.FindGameObjectWithTag(Tag.Fingerprint);

            var pointer = GameObject.FindGameObjectWithTag(Tag.FingerprintPointer);
            var mainTr = pointer.transform;
            var maskTr = mainTr.FindRecursiveByTag(Tag.FingerprintPointerMask);

            var pointerComponent = _world.CreateEntityWith<Fingerprint>();
            pointerComponent.Parent = fingerprint;
            pointerComponent.MainTransformForRotation = mainTr;
            pointerComponent.SpriteMaskTransform = maskTr;

            _maxScale = maskTr.localScale.y;

            fingerprint.SetActive(false);
        }
    }
}