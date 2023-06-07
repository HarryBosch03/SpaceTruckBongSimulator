using UnityEngine;
// ReSharper disable InconsistentNaming

namespace SpaceTruckBongSimulator.Interactables
{
    public interface IBehaviour
    {
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}