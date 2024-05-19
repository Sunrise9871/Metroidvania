using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class InputSystemOld : MonoBehaviour
    {
        #region Variables: Gravity
        private float _velocity;
        private float _gravity = -2f;
        #endregion

        #region Variables: Movement

        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private float jumpPower = 1.0f;

        private Vector2 _direction;

        #endregion

        private Vector2 _input;
        private CharacterController _characterController;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyMovement();
            ApplyGravity();
            _characterController.Move(_direction * Time.deltaTime);
        }
        

        public void JumpUp(InputAction.CallbackContext context)
        {
            if (context.started == false) return;
            if (_characterController.isGrounded == false) return;
            
            _velocity = jumpPower;
            Debug.Log("Velocity: " + _velocity + " Direction: " + _direction );
        }

        public void JumpOff(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
            Debug.Log("JumpOFF");
        }

        public void Move(InputAction.CallbackContext context)
        {
            //Считывает нажатия клавиатуры с помощью Player Input 
            _input = context.ReadValue<Vector2>();
        }

        private void ApplyMovement()
        {
            _direction = _input * (moveSpeed * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            //Если персонаж на земле
            if (_characterController.isGrounded && _velocity < 0.0f)
                _velocity = -1.0f;
            //Если в воздухе
            else
            {
                var previousYVelocity = _direction.y;
                var newYVelocity = _direction.y + (_gravity * Time.deltaTime);
                var nextYVelocity = (previousYVelocity + newYVelocity);
                _velocity = nextYVelocity;
            }
            
            _direction.y = _velocity;
        }
    }
}