using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public abstract class FloatDriver1D : MonoBehaviour
    {
        public abstract float Value { get; }
    }
}
