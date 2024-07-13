using Player.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Movement.PlayerInput;

namespace Player.Graphics
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerGraphics : MonoBehaviour
    {
        #region AnimationHash

        private readonly int _moveTrigger = Animator.StringToHash("Move");
        private readonly int _idleTrigger = Animator.StringToHash("Idle");
        private readonly int _jumpTrigger = Animator.StringToHash("Jump");
        private readonly int _landTrigger = Animator.StringToHash("Land");
        private readonly int _isFlying = Animator.StringToHash("IsFlying");
        private readonly int _speed = Animator.StringToHash("Speed");

        #endregion

        private PlayerInput _playerInput;
        private CharacterController2D _characterController2D;

        private Animator _animator;

        private bool _facingRight = true;


        private void Awake()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.PlayerInputActions.Player.Move.started += OnDirectMove;
            _playerInput.PlayerInputActions.Player.Move.canceled += OnDirectMove;

            _characterController2D.FlewUp += OnFlewUp;
            _characterController2D.Landed += OnLanded;
            _characterController2D.Jumped += OnJumped;
        }

        private void OnDisable()
        {
            _playerInput.PlayerInputActions.Player.Move.started -= OnDirectMove;
            _playerInput.PlayerInputActions.Player.Move.canceled -= OnDirectMove;

            _characterController2D.FlewUp -= OnFlewUp;
            _characterController2D.Landed -= OnLanded;
            _characterController2D.Jumped -= OnJumped;
        }

        private void OnIdled()
        {
            _animator.SetTrigger(_idleTrigger);
        }

        private void OnJumped()
        {
            _animator.SetTrigger(_jumpTrigger);
            _animator.ResetTrigger(_idleTrigger);
            _animator.ResetTrigger(_moveTrigger);
        }

        private void OnLanded()
        {
            print("landTrigger");
            _animator.SetBool(_isFlying, false);
            _animator.SetTrigger(_landTrigger);
        }

        private void OnMoved()
        {
            _animator.SetTrigger(_moveTrigger);
        }

        private void OnFlewUp()
        {
            _animator.SetBool(_isFlying, true);
        }

        private void OnDirectMove(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>().x;
            _animator.SetFloat(_speed, input);

            if (input > 0f && !_facingRight || input < 0f && _facingRight)
                Flip();

            if (!_characterController2D.IsGrounded) return;
            if (input == 0f)
            {
                OnIdled();
                print("idle");
            }
            else
            {
                OnMoved();
                print("move");
            }
        }

        private void Flip()
        {
            _facingRight = !_facingRight;

            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}