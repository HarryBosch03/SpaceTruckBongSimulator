using UnityEngine;

namespace SpaceTruckBongSimulator.Drivers
{
    public class SliderDriver1D : DraggableDriver
    {
        [SerializeField] private Transform handle;
        
        protected override void MoveHandle(float position)
        {
            if (!handle) return;
            handle.transform.position = transform.position + DragDirection.normalized * position;
        }
    }
}