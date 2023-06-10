using System;
using SpaceTruckBongSimulator.Interactables;
using UnityEngine;

namespace SpaceTruckBongSimulator.Player
{
    [Serializable]
    public class PlayerMovement
    {
        [SerializeField] private float moveSpeed = 12.0f;
        [SerializeField] private float accelerationTime = 1.0f;

        [Space] 
        [SerializeField] private float rotationSpring;
        [SerializeField] private float rotationDamper;
        
        private PlayerController controller;
        
        public void Init(PlayerController controller)
        {
            this.controller = controller;
        }

        public void FixedUpdate()
        {
            if (controller.CurrentMount != null)
            {
                ProcessMount();
                return;
            }
            Move();
            Rotate();
        }

        private void ProcessMount()
        {
            var transform = controller.transform;

            transform.position = controller.CurrentMount.Position;
            transform.rotation = controller.CurrentMount.Rotation;

            var rb = controller.Rigidbody;
            rb.isKinematic = true;
        }

        private void Rotate()
        {
            var rb = controller.Rigidbody;
            var target = controller.CameraContainer;

            var delta = target.rotation * Quaternion.Inverse(rb.rotation);
            delta.ToAngleAxis(out var angle, out var axis);

            var torque = axis * (angle * rotationSpring) - rb.angularVelocity * rotationDamper;
            rb.AddTorque(torque, ForceMode.Acceleration);
        }

        private void Move()
        {
            var rb = controller.Rigidbody;
            var transform = controller.CameraContainer;

            var direction = transform.TransformVector(controller.Input.MoveDirection);
            var target = direction * moveSpeed;

            var force = Vector3.ClampMagnitude(target - rb.velocity, moveSpeed) / accelerationTime;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }
}