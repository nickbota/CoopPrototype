using UnityEngine;

namespace CoopPrototype.Player
{
    public interface ICharacterMovement
    {
        void Move(Vector2 input);
        void Jump();
        bool IsGrounded { get; }
        float CurrentSpeed { get; }
    }
}
