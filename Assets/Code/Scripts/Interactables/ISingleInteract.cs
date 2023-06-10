using UnityEngine;

namespace SpaceTruckBongSimulator.Interactables
{
    public interface ISingleInteract : IInteractable
    {
        void Interact(GameObject interactor);
    }
}