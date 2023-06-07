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
        
        [Space]
        [SerializeField][Range(30.0f, 120.0f)] private float defaultFov = 90.0f; 
        [SerializeField][Range(30.0f, 120.0f)] private float zoomFov = 40.0f;
        [SerializeField] private float fovSmoothing = 0.1f; 
        
        private PlayerController controller;
        private float rollTime;
        private float smoothedRollInput;

        private Quaternion cameraRotation;
        private Camera camera;

        private bool zoom;
        private float tFov, cFov;
        
        public void Init(PlayerController controller)
        {
            this.controller = controller;
            
            cameraRotation = controller.CameraContainer.rotation;
            camera = Camera.main;
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
            if (controller.Input.Zoom == PlayerInput.Button.State.PressedThisFrame) zoom = !zoom;
            tFov = zoom ? zoomFov : defaultFov;
            
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

            camera.transform.position = controller.CameraContainer.position;
            camera.transform.rotation = controller.CameraContainer.rotation;

            cFov += fovSmoothing > 0.0f ? (tFov - cFov) / fovSmoothing * Time.deltaTime : tFov;
            camera.fieldOfView = cFov;
        }
    }
}