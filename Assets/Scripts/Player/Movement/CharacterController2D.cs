using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        private const float ApproximatelyZeroAcceleration = 0.001f;
        private const float GroundedDistance = 0.2f;

        [Range(0, 0.3f)] [Tooltip("Плавность движений")]
        [SerializeField] private float movementSmoothing = 0.05f;

        [Tooltip("Скорость бега")]
        [SerializeField] private float horizontalSpeed;

        [Tooltip("Сила прыжка")]
        [SerializeField] private float jumpForce = 400f;

        [Tooltip("Что является поверхностью для игрока")]
        [SerializeField] private LayerMask whatIsGround;

        [Tooltip("Ноги игрока")]
        [SerializeField] private Transform groundCheck;

        private Rigidbody2D _rigidbody2D;
        private Vector3 _velocity = Vector3.zero;
        private float _movementInput;

        private PlayerInput _playerInput;

        public bool IsGrounded { get; private set; }

        public event Action FlewUp;
        public event Action Landed;
        public event Action Jumped;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.PlayerInputActions.Player.JumpUp.started += OnJump;

            _playerInput.PlayerInputActions.Player.Move.started += OnMove;
            _playerInput.PlayerInputActions.Player.Move.canceled += OnMove;
        }

        private void OnDisable()
        {
            _playerInput.PlayerInputActions.Player.JumpUp.started -= OnJump;

            _playerInput.PlayerInputActions.Player.Move.started -= OnMove;
            _playerInput.PlayerInputActions.Player.Move.canceled -= OnMove;
        }

        private void FixedUpdate()
        {
            Move(_movementInput);
            GroundCheck();
        }

        private void Move(float move)
        {
            var targetVelocity = new Vector2(move * horizontalSpeed * Time.fixedDeltaTime,
                _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
                ref _velocity, movementSmoothing);
        }

        private void GroundCheck()
        {
            var wasGrounded = IsGrounded;
            IsGrounded = false;

            var groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, GroundedDistance,
                whatIsGround);

            if (!groundHit.collider || Mathf.Abs(_rigidbody2D.velocityY) > ApproximatelyZeroAcceleration)
            {
                FlewUp?.Invoke();
                print("flew up");
                return;
            }

            IsGrounded = true;
            if (!wasGrounded)
            {
                Landed?.Invoke();
                print("land");
            }
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded) return;
            _rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            Jumped?.Invoke();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.action.ReadValue<Vector2>().x;
        }
    }
}