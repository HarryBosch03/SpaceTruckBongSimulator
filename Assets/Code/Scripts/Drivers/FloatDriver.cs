using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    public abstract class FloatDriver : MonoBehaviour
    {
        protected abstract float Value { get; }
    }
}