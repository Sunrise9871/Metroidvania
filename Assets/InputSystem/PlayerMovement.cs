using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController2D controller;
        [SerializeField] private float speed; 

        private Vector2 _input;
        private float _horizontalMovement;
        private bool _isJumpPressed;
        private bool _isCrouchPressed;

        private void FixedUpdate()
        {
            controller.Move(_horizontalMovement * Time.fixedDeltaTime, _isCrouchPressed, _isJumpPressed);
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _input = context.ReadValue<Vector2>();
            _horizontalMovement = _input.x * speed;
        }
        
        public void OnJump(InputAction.CallbackContext context) => _isJumpPressed = context.ReadValueAsButton();
        public void OnCrouch(InputAction.CallbackContext context) => _isCrouchPressed = context.ReadValueAsButton();
    }
}
