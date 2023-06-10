using System;
using SpaceTruckBongSimulator.Interactables;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceTruckBongSimulator.Player
{
    [Serializable]
    public class PlayerInteractor
    {
        [SerializeField] private float interactionRange;
        [SerializeField] private PlayerInteractorCrosshair crosshair;

        private PlayerController controller;
        private InteractionState state = DefaultState;

        public static readonly InteractionState DefaultState = new InteractionState.Idle();
        
        public Ray Ray => new(controller.CameraContainer.position, controller.CameraContainer.forward);

        public void Init(PlayerController controller)
        {
            this.controller = controller;
        }

        public void Update()
        {
            crosshair.Update(GetLookingAt());
            
            if (state == null) state = DefaultState;
            state = state.Update(this);
        }

        public IInteractable GetLookingAt()
        {
            if (!Physics.Raycast(Ray, out var hit, interactionRange)) return null;
            return hit.transform.GetComponentInParent<IInteractable>();
        }
        
        public abstract class InteractionState
        {
            public abstract InteractionState Update(PlayerInteractor ctx);

            public class Idle : InteractionState
            {
                public override InteractionState Update(PlayerInteractor ctx)
                {
                    if (ctx.controller.Input.Interact != PlayerInput.Button.State.PressedThisFrame) return this;
                    
                    var lookingAt = ctx.GetLookingAt();
                    if (!(lookingAt as Object)) return this;

                    switch (lookingAt)
                    {
                        case IDraggable draggable:
                            return new Dragging(ctx, draggable);
                        case ISingleInteract single:
                            single.Interact(ctx.controller.gameObject);
                            break;
                        case IMountable mountable:
                            ctx.controller.CurrentMount = mountable;
                            break;
                    }
                    return this;
                }
            }

            public class Dragging : InteractionState
            {
                private readonly IDraggable draggable;

                public Dragging(PlayerInteractor ctx, IDraggable draggable)
                {
                    this.draggable = draggable;
                    draggable.StartDrag(ctx.Ray);
                }
                
                public override InteractionState Update(PlayerInteractor ctx)
                {
                    if (ctx.controller.Input.Interact == PlayerInput.Button.State.ReleasedThisFrame)
                    {
                        draggable.EndDrag(ctx.Ray);
                        return DefaultState;
                    }
                    draggable.SustainDrag(ctx.Ray);
                    return this;
                }
            }
        }
    }
}