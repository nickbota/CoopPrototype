using UnityEngine;
using UnityEngine.InputSystem;

namespace CoopPrototype.Player
{
    public class PlayerInputHandler : MonoBehaviour, IMovementInput
    {
        public Vector2 MovementInput { get; private set; }
        public bool IsJumpPressed { get; private set; }

        public void OnMove(InputValue value)
        {
            MovementInput = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            IsJumpPressed = value.isPressed;
        }
    }
}
