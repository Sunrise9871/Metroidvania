using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        private const float GroundedDistance = 0.2f; // Дальность ray cast для ground check

        [Range(0, .3f)] [Tooltip("Плавность движений")]
        [SerializeField] private float movementSmoothing = 0.05f;

        [Tooltip("Скорость бега")]
        [SerializeField] private float horizontalSpeed;

        [Tooltip("Сила прыжка")]
        [SerializeField] private float jumpForce = 400f;

        [Tooltip("Что является поверхностью для игрока")]
        [SerializeField] private LayerMask whatIsGround;

        [Tooltip("Ноги игрока")]
        [SerializeField] private Transform groundCheck;

        private bool _grounded;
        private Rigidbody2D _rigidbody2D;
        private Vector3 _velocity = Vector3.zero;
        private Vector2 _movementInput;

        private PlayerInput _playerInput;

        public event Action PlayerLanded;
        public event Action PlayerFlying;

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
            Move(_movementInput.x);

            var wasGrounded = _grounded;
            _grounded = false;

            var groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, GroundedDistance,
                whatIsGround);
            if (!groundHit.collider)
            {
                PlayerFlying?.Invoke();
                return;
            }

            _grounded = true;
            if (!wasGrounded)
                PlayerLanded?.Invoke();
        }

        private void Move(float move)
        {
            var targetVelocity = new Vector2(move * horizontalSpeed * Time.fixedDeltaTime,
                _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
                ref _velocity, movementSmoothing);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (!_grounded) return;

            if (_rigidbody2D.velocityY < 0.01f)
                _rigidbody2D.AddForce(new Vector2(0f, jumpForce));

            _grounded = true;
        }

        private void OnMove(InputAction.CallbackContext context) =>
            _movementInput = context.action.ReadValue<Vector2>();
    }
}