using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTruckBongSimulator.Player
{
    [Serializable]
    public class PlayerCamera
    {
        [SerializeField] private float mouseSensitivity;   
        [SerializeField] private AnimationCurve rollSensitivity; 
        [SerializeField] private float rollSensitivityScale;
        [SerializeField] private float rollInputSmoothing; 
        
        private PlayerController controller;
        private float rollTime;
        private float smoothedRollInput;

        private Quaternion cameraRotation;
        
        public void Init(PlayerController controller)
        {
            this.controller = controller;
            
            cameraRotation = controller.CameraContainer.rotation;
        }

        public void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
        public void LateUpdate()
        {
            var delta = Vector3.zero;

            if (Mouse.current != null)
            {
                delta += (Vector3)Mouse.current.delta.ReadValue() * mouseSensitivity;
            }

            var roll = controller.Input.Roll;
            if (roll * roll < 0.01f)
            {
                rollTime = Time.time;
            }

            if (smoothedRollInput == 0.0f) smoothedRollInput = roll;
            else smoothedRollInput += (roll - smoothedRollInput) / rollInputSmoothing * Time.deltaTime;
            
            delta.z += smoothedRollInput * rollSensitivity.Evaluate(Time.time - rollTime) * rollSensitivityScale * Time.deltaTime;
            
            var deltaRotation = Quaternion.Euler(-delta.y, delta.x, -delta.z);
            cameraRotation *= deltaRotation;
            controller.CameraContainer.rotation = cameraRotation;
        }
    }
}