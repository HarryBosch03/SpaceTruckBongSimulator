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
            moveX,
            moveY,
            moveZ,
            roll;

        private Button[] buttons = new[]
        {
            new Button("Interact"),
            new Button("Zoom"),
        };

        public Vector3 MoveDirection => Vector3.ClampMagnitude(new Vector3
        {
            x = moveX.action.ReadValue<float>(),
            y = moveY.action.ReadValue<float>(),
            z = moveZ.action.ReadValue<float>(),
        }, 1.0f);

        public float Roll => roll.action.ReadValue<float>();
        public Button.State Interact => buttons[0];
        public Button.State Zoom => buttons[1];

        public void Init(PlayerController controller)
        {
            this.controller = controller;

            inputAsset.Enable();

            InputActionReference bind(string name)
            {
                return InputActionReference.Create(inputAsset.FindAction(name));
            }

            moveX = bind("Move X");
            moveY = bind("Move Y");
            moveZ = bind("Move Z");
            roll = bind("Roll");

            foreach (var button in buttons)
            {
                button.Bind(bind);
            }
        }

        public void Update()
        {
            foreach (var button in buttons)
            {
                button.Update();
            }
        }
        
        public class Button
        {
            public State state;
            public InputActionReference actionReference;

            private readonly string actionName;
            private bool lastState;

            public Button(string actionName)
            {
                this.actionName = actionName;
            }

            public void Bind(Func<string, InputActionReference> bindCallback)
            {
                actionReference = bindCallback(actionName);
            }
            
            public void Update()
            {
                var thisState = actionReference.action.ReadValue<float>() > 0.5f;
                state = (State)((thisState ? 0b10 : 0b00) + (lastState ? 0b01 : 0b00));
                lastState = thisState;
            }

            public static implicit operator State(Button button)
            {
                return button == null ? default : button.state;
            }
            
            // Layout 0b[This Frame State][Last Frame State]
            public enum State
            {
                Released = default,
                ReleasedThisFrame = 0b01,
                PressedThisFrame = 0b10,
                Pressed = 0b11,
            }
        }
    }
}