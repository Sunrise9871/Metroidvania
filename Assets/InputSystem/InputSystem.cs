using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class InputSystem : MonoBehaviour
    {
        //gravity variables
        [SerializeField] private float fallMultiplier = 2.0f;
        [SerializeField] private float maxFallingSpeed = -30.0f;
        private float _gravity;
        private const float GROUNDED_GRAVITY = -.05f;

        //jumping variables
        [SerializeField] private float maxJumpHeight = 6.0f;
        [SerializeField] private float maxJumpTime = 0.75f;
        private bool _isJumpPressed;
        private bool _isJumping;
        private float _initialJumpVelocity;

        //movement variables
        [SerializeField] private float speed;
        private Vector2 _currentMovement;
        private Vector2 _appliedMovement;
        
        private CharacterController _characterController;
        private Vector2 _input;
        

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            SetupJumpVariables();
        }

        private void Update()
        {
            _appliedMovement.x = _currentMovement.x;
            _characterController.Move(_appliedMovement * Time.deltaTime);

            HandleGravity();
            HandleJump();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _input = context.ReadValue<Vector2>();
            _currentMovement.x = _input.x * speed;
            if (context.performed)
                HandleRotation();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _isJumpPressed = context.ReadValueAsButton();
        }

        private void HandleJump()
        {
            if (_isJumping == false && _characterController.isGrounded && _isJumpPressed)
            {
                _isJumping = true;
                _currentMovement.y = _initialJumpVelocity;
                _appliedMovement.y = _initialJumpVelocity;
            }
            else if (_isJumpPressed == false && _isJumping && _characterController.isGrounded)
            {
                _isJumping = false;
            }
        }

        private void HandleGravity()
        {
            var isFalling = _currentMovement.y <= 0.0f || _isJumpPressed == false;

            if (_characterController.isGrounded)
            {
                _currentMovement.y = GROUNDED_GRAVITY;
                _appliedMovement.y = GROUNDED_GRAVITY;
            }
            else if (isFalling)
            {
                var previousYVelocity = _currentMovement.y;
                _currentMovement.y += _gravity * fallMultiplier * Time.deltaTime;
                _appliedMovement.y = Mathf.Max((previousYVelocity + _currentMovement.y) * .5f, maxFallingSpeed);
            }
            else
            {
                var previousYVelocity = _currentMovement.y;
                _currentMovement.y += _gravity * Time.deltaTime;
                _appliedMovement.y = (previousYVelocity + _currentMovement.y) * .5f;
            }
        }

        private void SetupJumpVariables()
        {
            var timeToApex = maxJumpTime / 2f;
            _gravity = -2f * maxJumpHeight / (timeToApex * timeToApex);
            _initialJumpVelocity = 2f * maxJumpHeight / timeToApex;
        }

        private void HandleRotation()
        {
            //Для плавного поворота:
            //https://youtu.be/bXNFxQpp2qk?si=1DwCot83CUoPo98q&t=1328
            gameObject.transform.rotation = _input.x switch
            {
                > 0 => Quaternion.Euler(0, 0, 0),
                < 0 => Quaternion.Euler(0, 180, 0),
                _ => transform.rotation
            };
        }
    }
}