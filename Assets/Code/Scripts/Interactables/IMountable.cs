using UnityEngine;

namespace SpaceTruckBongSimulator.Interactables
{
    public interface IMountable : IInteractable
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        bool LimitLookRotation { get; }
        Quaternion LookRotationBase { get; }
        Vector2 LookRotationOffset { get; }
        Vector2 LookRotationLimit { get; }
    }
}