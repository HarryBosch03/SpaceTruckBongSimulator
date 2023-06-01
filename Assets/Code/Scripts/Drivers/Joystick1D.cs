using System;
using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class Joystick1D : FloatDriver1D, IBindable
    {
        [SerializeField] private float maxAngle;
        [SerializeField] private Vector3 rotationAxis;
        [SerializeField] private Vector3 planeAxis;
        [SerializeField] private Transform handle;

        private float angle;
        private Binding binding;
        
        public override float Value => Mathf.InverseLerp(-maxAngle, maxAngle, angle);
        public bool CanBind => true;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!binding) return;

            var ray = new Ray(binding.target.BindingPosition, binding.target.BindingRotation * Vector3.forward);
            var point = handle.transform.position;

            var vector = point - ray.origin;
            var dot = Vector3.Dot(vector, ray.direction);
            
        }

        public Binding Bind(IBindingTarget target)
        {
            binding = new Binding(target);
            return binding;
        }

        public void BindingDeactivated(IBindingTarget oldBinding) { }
    }
}
