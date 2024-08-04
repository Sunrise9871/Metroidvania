using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Control
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        private const float ApproximatelyZeroAcceleration = 0.001f;
        private const float GroundedDistance = 0.2f;

        [Header("Основные")]
        [Tooltip("Скорость бега")]
        [SerializeField] private float horizontalSpeed;

        [Tooltip("Сила прыжка")]
        [SerializeField] private float jumpForce = 400f;

        [Header("Способности")]
        [Tooltip("Сила толчка")]
        [SerializeField] private float dashForce = 25f;

        [Tooltip("Время толчка")]
        [SerializeField] private float dashTime = 0.2f;

        [Header("Технические")]
        [Tooltip("Что является поверхностью для игрока")]
        [SerializeField] private LayerMask whatIsGround;

        [Tooltip("Ноги игрока")]
        [SerializeField] private Transform groundCheck;

        private Rigidbody2D _rigidbody2D;
        private PlayerInput _playerInput;

        private Vector3 _velocity;
        private float _movementInput;

        private bool _canDash = true;
        private bool _isDashing;

        public bool IsGrounded { get; private set; }

        public event Action FlewUp;
        public event Action Landed;
        public event Action Jumped;
        public event Action<bool> DashStateChanged;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.PlayerInputActions.Player.JumpUp.started += OnJump;
            _playerInput.PlayerInputActions.Player.Dash.started += OnDash;

            _playerInput.PlayerInputActions.Player.Move.started += OnMove;
            _playerInput.PlayerInputActions.Player.Move.canceled += OnMove;
        }

        private void OnDisable()
        {
            _playerInput.PlayerInputActions.Player.JumpUp.started -= OnJump;
            _playerInput.PlayerInputActions.Player.Dash.started -= OnDash;

            _playerInput.PlayerInputActions.Player.Move.started -= OnMove;
            _playerInput.PlayerInputActions.Player.Move.canceled -= OnMove;
        }

        private void FixedUpdate()
        {
            Move();
            GroundCheck();
        }

        private void Move()
        {
            if (_isDashing) return;

            var targetVelocity = new Vector2(_movementInput * horizontalSpeed * Time.fixedDeltaTime,
                _rigidbody2D.linearVelocity.y);

            _rigidbody2D.linearVelocity = targetVelocity;
        }

        private void GroundCheck()
        {
            var wasGrounded = IsGrounded;
            IsGrounded = false;

            var groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, GroundedDistance,
                whatIsGround);

            if (!groundHit.collider || Mathf.Abs(_rigidbody2D.linearVelocityY) > ApproximatelyZeroAcceleration)
            {
                FlewUp?.Invoke();
                return;
            }
            IsGrounded = true;

            if (wasGrounded) return;
            Landed?.Invoke();
            _canDash = true;
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded) return;
            
            _rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            Jumped?.Invoke();
        }

        private void OnMove(InputAction.CallbackContext context) =>
            _movementInput = context.action.ReadValue<Vector2>().x;

        private void OnDash(InputAction.CallbackContext context)
        {
            if (_canDash && !IsGrounded && _movementInput != 0)
                StartCoroutine(Dash());
        }

        private IEnumerator Dash()
        {
            _canDash = false;
            _isDashing = true;
            var originalGravity = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.linearVelocity = new Vector2(_movementInput * dashForce, 0f);
            DashStateChanged?.Invoke(true);

            yield return new WaitForSeconds(dashTime);
            _rigidbody2D.gravityScale = originalGravity;
            _isDashing = false;
            DashStateChanged?.Invoke(false);
        }
    }
}