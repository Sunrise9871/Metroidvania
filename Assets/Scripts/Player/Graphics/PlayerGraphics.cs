using Player.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Movement.PlayerInput;

namespace Player.Graphics
{
    [RequireComponent(typeof(Animator))]
    public class PlayerGraphics : MonoBehaviour
    {
        #region AnimationHash

        private readonly int _moveTrigger = Animator.StringToHash("Move");
        private readonly int _idleTrigger = Animator.StringToHash("Idle");
        private readonly int _jumpTrigger = Animator.StringToHash("Jump");
        private readonly int _landTrigger = Animator.StringToHash("Land");
        private readonly int _flyTrigger = Animator.StringToHash("Fly");

        #endregion


        private PlayerInput _playerInput;
        private CharacterController2D _characterController2D;
        private Animator _animator;

        private bool _facingRight = true;


        private void Awake()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
            _characterController2D = GetComponentInParent<CharacterController2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _playerInput.PlayerInputActions.Player.Move.started += OnMove;
            _playerInput.PlayerInputActions.Player.Move.canceled += OnMove;

            _characterController2D.PlayerLanded += OnLand;
            _characterController2D.PlayerFlying += OnFly;
        }

        private void OnDisable()
        {
            _playerInput.PlayerInputActions.Player.Move.started -= OnMove;
            _playerInput.PlayerInputActions.Player.Move.canceled -= OnMove;

            _characterController2D.PlayerLanded -= OnLand;
            _characterController2D.PlayerFlying -= OnFly;
        }

        private void Idle() => _animator.SetTrigger(_idleTrigger);

        public void OnJump(InputAction.CallbackContext context)
        {
            _animator.SetTrigger(_jumpTrigger);
        }

        private void OnLand()
        {
            _animator.SetTrigger(_landTrigger);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var input = context.action.ReadValue<Vector2>().x;
            
            if (input > 0 && !_facingRight || input < 0 && _facingRight)
                Flip();

            _animator.SetTrigger(input == 0 ? _idleTrigger : _moveTrigger);
        }

        private void OnFly()
        {
            _animator.SetTrigger(_flyTrigger);
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