using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTruckBongSimulator.Player
{
    [Serializable]
    public class PlayerInput
    {
        [SerializeField] private InputActionAsset inputAsset;
        
        private PlayerController controller;

        private InputActionReference 
            moveX, moveY, moveZ, 
            roll;

        public Vector3 MoveDirection => Vector3.ClampMagnitude(new Vector3
        {
            x = moveX.action.ReadValue<float>(),
            y = moveY.action.ReadValue<float>(),
            z = moveZ.action.ReadValue<float>(),
        }, 1.0f);

        public float Roll => roll.action.ReadValue<float>();
        
        public void Init(PlayerController controller)
        {
            this.controller = controller;

            inputAsset.Enable();

            void bind(out InputActionReference target, string name)
            {
                target = InputActionReference.Create(inputAsset.FindAction(name));
            }
            
            bind(out moveX, "Move X");
            bind(out moveY, "Move Y");
            bind(out moveZ, "Move Z");
            bind(out roll, "Roll");
        }
    }
}