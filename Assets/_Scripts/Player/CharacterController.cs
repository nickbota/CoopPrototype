using UnityEngine;

namespace CoopPrototype.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(CharacterMovementController))]
    public class CharacterController : MonoBehaviour
    {
        private IMovementInput inputHandler;
        private ICharacterMovement movementController;

        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
            movementController = GetComponent<CharacterMovementController>();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleJump();
        }

        private void HandleMovement()
        {
            Vector2 input = inputHandler.MovementInput;
            movementController.Move(input);
        }

        private void HandleJump()
        {
            if (inputHandler.IsJumpPressed)
            {
                movementController.Jump();
            }
        }
    }
}