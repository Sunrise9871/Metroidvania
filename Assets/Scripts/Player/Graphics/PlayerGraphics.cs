using Player.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Movement.PlayerInput;

namespace Player.Graphics
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerGraphics : MonoBehaviour
    {
        #region AnimationHash

        private readonly int _moveTrigger = Animator.StringToHash("Move");
        private readonly int _idleTrigger = Animator.StringToHash("Idle");
        private readonly int _jumpTrigger = Animator.StringToHash("Jump");
        private readonly int _landTrigger = Animator.StringToHash("Land");
        private readonly int _isDashing = Animator.StringToHash("IsDashing");
        private readonly int _isFlying = Animator.StringToHash("IsFlying");
        private readonly int _speed = Animator.StringToHash("Speed");
        
        #endregion

        private PlayerInput _playerInput;
        private CharacterController2D _characterController2D;
        private SpriteRenderer _spriteRenderer;

        private Animator _animator;

        private void Awake()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _playerInput.PlayerInputActions.Player.Move.started += OnDirectMove;
            _playerInput.PlayerInputActions.Player.Move.canceled += OnDirectMove;

            _characterController2D.FlewUp += OnFlewUp;
            _characterController2D.Landed += OnLanded;
            _characterController2D.Jumped += OnJumped;
            _characterController2D.DashStateChanged += OnDashStateChanged;
        }

        private void OnDisable()
        {
            _playerInput.PlayerInputActions.Player.Move.started -= OnDirectMove;
            _playerInput.PlayerInputActions.Player.Move.canceled -= OnDirectMove;

            _characterController2D.FlewUp -= OnFlewUp;
            _characterController2D.Landed -= OnLanded;
            _characterController2D.Jumped -= OnJumped;
            _characterController2D.DashStateChanged -= OnDashStateChanged;
        }

        private void OnIdled() => _animator.SetTrigger(_idleTrigger);

        private void OnJumped() => _animator.SetTrigger(_jumpTrigger);

        private void OnMoved() => _animator.SetTrigger(_moveTrigger);

        private void OnFlewUp() => _animator.SetBool(_isFlying, true);
        
        private void OnLanded()
        {
            _animator.SetBool(_isFlying, false);
            _animator.SetTrigger(_landTrigger);
            _animator.ResetTrigger(_idleTrigger);
            _animator.ResetTrigger(_moveTrigger);
        }

        private void OnDashStateChanged(bool state)
        {
            _animator.SetBool(_isDashing, state);
            FlipSprite(_animator.GetFloat(_speed));
        }

        private void OnDirectMove(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>().x;
            _animator.SetFloat(_speed, input);

            if (_animator.GetBool(_isDashing)) return;
            
            FlipSprite(input);

            if (!_characterController2D.IsGrounded) return;
            if (input == 0f)
                OnIdled();
            else
                OnMoved();
        }

        private void FlipSprite(float input)
        {
            if ((input > 0f || !_spriteRenderer.flipX) && (input < 0f || _spriteRenderer.flipX)) 
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }
}