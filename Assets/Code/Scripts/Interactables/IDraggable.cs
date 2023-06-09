using UnityEngine;

namespace SpaceTruckBongSimulator.Interactables
{
    public interface IDraggable : IInteractable
    {
        void StartDrag(Ray ray);
        void SustainDrag(Ray ray);
        void EndDrag(Ray ray);
    }
}
