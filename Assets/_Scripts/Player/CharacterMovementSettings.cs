using UnityEngine;

namespace CoopPrototype.Player
{
    [CreateAssetMenu(fileName = "CharacterMovementSettings", menuName = "Coop Prototype/Character Movement Settings")]
    public class CharacterMovementSettings : ScriptableObject
    {
        [Header("Movement")]
        [Min(0f)] public float moveSpeed = 5f;
        [Min(0f)] public float acceleration = 10f;
        [Min(0f)] public float deceleration = 10f;
        [Min(0f)] public float rotationSpeed = 10f;

        [Header("Jump")]
        [Min(0f)] public float jumpForce = 5f;
        [Min(0f)] public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer;
    }
}
