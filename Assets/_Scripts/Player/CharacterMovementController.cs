using UnityEngine;

namespace CoopPrototype.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterMovementController : MonoBehaviour, ICharacterMovement
    {
        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;

        [Header("Settings")]
        [SerializeField] private CharacterMovementSettings settings;

        //Movement default values
        private float moveSpeed = 5f;
        private float acceleration = 10f;
        private float deceleration = 10f;
        private float rotationSpeed = 10f;

        //Jumping and Ground Check default values
        private float jumpForce = 5f;
        private float groundCheckDistance = 0.1f;
        private LayerMask groundLayer;

        [Header("Physics Settings")]
        [SerializeField] private float gravityMultiplier = 2.5f;
        [SerializeField] private float groundingForce = 5f;

        private float MoveSpeed => settings != null ? settings.moveSpeed : moveSpeed;
        private float Acceleration => settings != null ? settings.acceleration : acceleration;
        private float Deceleration => settings != null ? settings.deceleration : deceleration;
        private float RotationSpeed => settings != null ? settings.rotationSpeed : rotationSpeed;
        private float JumpForce => settings != null ? settings.jumpForce : jumpForce;
        private float GroundCheckDistance => settings != null ? settings.groundCheckDistance : groundCheckDistance;
        private LayerMask GroundLayer => settings != null ? settings.groundLayer : groundLayer;

        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;
        private Vector3 currentVelocity;
        private bool jumpRequested;

        public bool IsGrounded { get; private set; }
        public float CurrentSpeed => new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            rb.freezeRotation = true;

            if (cameraTransform == null)
            {
                cameraTransform = UnityEngine.Camera.main?.transform;
            }
        }

        private void FixedUpdate()
        {
            CheckGroundStatus();
            HandleJump();
            ApplyGravityModifier();
        }

        public void Move(Vector2 input)
        {
            if (input.sqrMagnitude > 0.01f)
            {
                Vector3 moveDirection = GetCameraRelativeMovement(input);
                Vector3 targetVelocity = moveDirection * MoveSpeed;
                
                currentVelocity = Vector3.Lerp(
                    new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z),
                    targetVelocity,
                    Acceleration * Time.fixedDeltaTime
                );
                
                RotateTowardsMovement(moveDirection);
            }
            else
            {
                currentVelocity = Vector3.Lerp(
                    new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z),
                    Vector3.zero,
                    Deceleration * Time.fixedDeltaTime
                );
            }

            rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
        }

        public void Jump()
        {
            jumpRequested = true;
        }
        private void HandleJump()
        {
            if (jumpRequested && IsGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                jumpRequested = false;
            }
            else if (!IsGrounded)
            {
                jumpRequested = false;
            }
        }
        private void ApplyGravityModifier()
        {
            if (!IsGrounded && rb.linearVelocity.y < 0)
                rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
            else if (IsGrounded && rb.linearVelocity.y <= 0.1f)
                rb.AddForce(Vector3.down * groundingForce, ForceMode.Acceleration);
        }
        private void CheckGroundStatus()
        {
            Vector3 spherePosition = transform.position + Vector3.up * capsuleCollider.radius;
            float checkDistance = capsuleCollider.bounds.extents.y - capsuleCollider.radius + GroundCheckDistance;
            
            IsGrounded = Physics.SphereCast(
                spherePosition,
                capsuleCollider.radius * 0.9f,
                Vector3.down,
                out RaycastHit hitInfo,
                checkDistance,
                GroundLayer,
                QueryTriggerInteraction.Ignore
            );
        }

        private Vector3 GetCameraRelativeMovement(Vector2 input)
        {
            if (cameraTransform == null)
            {
                return new Vector3(input.x, 0f, input.y).normalized;
            }

            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            return (cameraForward * input.y + cameraRight * input.x).normalized;
        }

        private void RotateTowardsMovement(Vector3 direction)
        {
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotationSpeed * Time.fixedDeltaTime
                );
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (capsuleCollider == null)
                capsuleCollider = GetComponent<CapsuleCollider>();

            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Vector3 spherePosition = transform.position + Vector3.up * capsuleCollider.radius;
            float checkDistance = capsuleCollider.bounds.extents.y - capsuleCollider.radius + GroundCheckDistance;
            Gizmos.DrawWireSphere(spherePosition + Vector3.down * checkDistance, capsuleCollider.radius * 0.9f);
        }
    }
}
