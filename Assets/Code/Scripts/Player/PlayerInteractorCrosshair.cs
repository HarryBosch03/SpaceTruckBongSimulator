using System;
using SpaceTruckBongSimulator.Interactables;
using UnityEngine;

namespace SpaceTruckBongSimulator.Player
{
    [Serializable]
    public class PlayerInteractorCrosshair
    {
        [SerializeField] private Transform scaleHandle;
        [SerializeField] private float idleScale;
        [SerializeField] private float lookingAtScale;
        [SerializeField] private float spring;
        [SerializeField] private float damper;

        private float position;
        private float velocity;
        
        public void Update(IInteractable lookingAt)
        {
            var t = lookingAt != null ? 1.0f : 0.0f;
            var force = (t - position) * spring - velocity * damper;
            position += velocity * Time.deltaTime;
            velocity += force * Time.deltaTime;

            if (scaleHandle) scaleHandle.localScale = Vector3.one * Mathf.LerpUnclamped(idleScale, lookingAtScale, position);
        }
    }
}