using SpaceTruckBongSimulator.Interactables;
using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    public abstract class DraggableDriver : FloatDriver, IDraggable
    {
        [SerializeField] protected Vector3 localDragDirection = Vector3.forward;
        [SerializeField] protected float range = 1.0f;
        [SerializeField] protected float handlePosition;

        protected sealed override float Value => Mathf.InverseLerp(-range, range, handlePosition);
        public bool CanInteract => true;

        private float offset;
        public Vector3 DragDirection => transform.TransformVector(localDragDirection).normalized;
        
        protected abstract void MoveHandle(float position);
        
        private float GetGrabPosition(Ray ray)
        {
            var root = transform.position;
            var direction = (ray.origin - root).normalized;
            var plane = new Plane(direction, root);
            if (!plane.Raycast(ray, out var enter)) return 0.0f;

            var point = ray.GetPoint(enter);
            return Vector3.Dot(DragDirection.normalized, point - root);
        }

        public virtual void StartDrag(Ray ray)
        {
            offset = GetGrabPosition(ray) - handlePosition;
        }

        public virtual void SustainDrag(Ray ray)
        {
            var newPosition = GetGrabPosition(ray);
            handlePosition = Mathf.Clamp(newPosition - offset, -range, range);
            MoveHandle(handlePosition);
        }

        public virtual void EndDrag(Ray ray)
        {
            
        }

        private void OnValidate()
        {
            MoveHandle(handlePosition);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;

            var a = localDragDirection.normalized * -range;
            var b = localDragDirection.normalized * range;
            
            Gizmos.DrawLine(a, b);
            BetterGizmos.DrawWireCircle(a, localDragDirection.normalized, range * 0.2f);
            BetterGizmos.DrawWireCircle(b, localDragDirection.normalized, range * 0.2f);
        }
    }
}