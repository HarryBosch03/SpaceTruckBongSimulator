using SpaceTruckBongSimulator.Utility;
using UnityEngine;

namespace SpaceTruckBongSimulator.Interactables
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class PressButton : MonoBehaviour, ISingleInteract
    {
        [SerializeField] private Transform handle;
        [SerializeField] private Vector3 defaultPosition;
        [SerializeField] private Vector3 pressPosition;

        [Space] 
        [SerializeField] private SpringDamperF handleSpring;
        [SerializeField] private float pressImpulse;

        public bool CanInteract => true;

        private void FixedUpdate()
        {
            handleSpring.Update(1.0f);
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (!handle) return;
            handle.localPosition = Vector3.Lerp(pressPosition, defaultPosition, handleSpring.position);
        }

        public void Interact(GameObject interactor)
        {
            handleSpring.AddImpulse(-pressImpulse);
        }

        private void OnValidate()
        {
            handleSpring.useLimits = true;
            handleSpring.limits = Vector2.right;
        }
    }
}
