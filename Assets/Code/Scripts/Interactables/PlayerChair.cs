using SpaceTruckBongSimulator.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTruckBongSimulator.Interactables
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class PlayerChair : MonoBehaviour, IMountable
    {
        [SerializeField] private Transform anchorPoint;
        [SerializeField] private bool lockLookRotation;
        [SerializeField] private Vector2 lookRotationOffset;
        [SerializeField] private Vector2 lookRotationLimit;

        public bool CanInteract => true;
        public Vector3 Position => anchorPoint.position;
        public Quaternion Rotation => anchorPoint.rotation;
        public bool LimitLookRotation => lockLookRotation;
        public Quaternion LookRotationBase => anchorPoint.rotation;
        public Vector2 LookRotationOffset => lookRotationOffset;
        public Vector2 LookRotationLimit => lookRotationLimit;
    }
}
