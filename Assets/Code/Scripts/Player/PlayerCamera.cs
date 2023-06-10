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

        [Space] [SerializeField] [Range(30.0f, 120.0f)]
        private float defaultFov = 90.0f;

        [SerializeField] [Range(30.0f, 120.0f)]
        private float zoomFov = 40.0f;

        [SerializeField] private float fovSmoothing = 0.1f;

        private PlayerController controller;
        private float rollTime;
        private float smoothedRollInput;

        private Quaternion cameraRotation;
        private Camera camera;

        private Vector2 mountRotation;
        private bool wasMounted;

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
            UpdateZoom();
            UpdateCameraRotation();
            ApplyPropertiesToCamera();

            wasMounted = controller.CurrentMount != null;
        }

        private void UpdateZoom()
        {
            if (controller.Input.Zoom == PlayerInput.Button.State.PressedThisFrame) zoom = !zoom;
            tFov = zoom ? zoomFov : defaultFov;
        }

        private void ApplyPropertiesToCamera()
        {
            controller.CameraContainer.rotation = cameraRotation;

            camera.transform.position = controller.CameraContainer.position;
            camera.transform.rotation = controller.CameraContainer.rotation;

            cFov += fovSmoothing > 0.0f ? (tFov - cFov) / fovSmoothing * Time.deltaTime : tFov;
            camera.fieldOfView = cFov;
        }

        private void UpdateCameraRotation()
        {
            if (controller.CurrentMount != null)
            {
                UpdateCameraRotationWithMount();
                return;
            }

            var delta = GetDelta();

            var deltaRotation = Quaternion.Euler(-delta.y, delta.x, -delta.z);
            cameraRotation *= deltaRotation;
        }

        private Vector3 GetDelta()
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
            return delta;
        }

        private void UpdateCameraRotationWithMount()
        {
            if (!wasMounted)
            {
                mountRotation = Vector2.zero;
            }

            var delta = GetDelta();
            mountRotation += new Vector2(delta.x, delta.y);
            var mount = controller.CurrentMount;
            
            if (mount.LimitLookRotation)
            {
                mountRotation -= mount.LookRotationOffset;
                
                var limit = mount.LookRotationLimit;
                mountRotation.x = Mathf.Clamp(mountRotation.x, -limit.x / 2.0f, limit.x / 2.0f);
                mountRotation.y = Mathf.Clamp(mountRotation.y, -limit.y / 2.0f, limit.y / 2.0f);
                
                mountRotation += mount.LookRotationOffset;
            }
            else
            {
                mountRotation.y = Mathf.Clamp(mountRotation.y, -90.0f, 90.0f);
            }

            cameraRotation = mount.LookRotationBase * Quaternion.Euler(-mountRotation.y, mountRotation.x, 0.0f);
        }
    }
}