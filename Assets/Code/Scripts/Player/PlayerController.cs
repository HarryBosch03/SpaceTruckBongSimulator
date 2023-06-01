using UnityEngine;

namespace SpaceTruckBongSimulator.Player
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private new PlayerCamera camera;
        
        public PlayerInput Input => input;
        public PlayerMovement Movement => movement;
        public PlayerCamera Camera => camera;
        
        public Transform CameraContainer { get; private set; }
        
        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();

            CameraContainer = transform.Find("Camera Container");
        }

        private void OnEnable()
        {
            camera.OnEnable();
        }

        private void OnDisable()
        {
            camera.OnDisable();
        }

        private void Start()
        {
            input.Init(this);
            movement.Init(this);
            camera.Init(this);
        }

        private void FixedUpdate()
        {
            movement.FixedUpdate();
        }

        private void LateUpdate()
        {
            camera.LateUpdate();
        }
    }
}
