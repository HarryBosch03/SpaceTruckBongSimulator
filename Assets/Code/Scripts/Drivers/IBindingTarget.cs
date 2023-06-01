using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    public interface IBindingTarget
    {
        Vector3 BindingPosition { get; }
        Quaternion BindingRotation { get; }
        
        void BindingDeactivated(IBindingTarget oldBinding);
    }
}