using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    public class JoystickDriver1D : DraggableDriver
    {
        [SerializeField] private Transform handle;
        [SerializeField] private Vector3 rotationAxis = Vector3.right;
        [SerializeField] private float maxAngle = 35.0f;

        protected override void MoveHandle(float position)
        {
            if (!handle) return;
            var sinAngle = Mathf.InverseLerp(-range, range, position) * 2.0f - 1.0f;
            var angle = Mathf.Asin(sinAngle * Mathf.Sin(maxAngle * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
            handle.localRotation = Quaternion.Euler(rotationAxis * angle);
        }
    }
}