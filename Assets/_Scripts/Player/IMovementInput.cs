using UnityEngine;

namespace CoopPrototype.Player
{
    public interface IMovementInput
    {
        Vector2 MovementInput { get; }
        bool IsJumpPressed { get; }
    }
}
