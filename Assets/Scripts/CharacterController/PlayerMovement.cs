using System;
using Shooting.Bullets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController2D controller;
        [SerializeField] private float speed;

        private Vector2 _input;
        private float _horizontalMovement;

        private bool _isJumpPressed;
        private bool _isCrouchPressed;
        private bool _isPrimaryFirePressed;
        private bool _isSecondaryFirePressed;
        private bool _isCombinedFirePressed;
        private bool _wasCombinedFirePressed;

        private PlayerShootProjectiles _playerShootProjectiles;
        private Style _style;

        public event Action PlayerMoved;
        public event Action PlayerIdled;

        private void Awake()
        {
            _playerShootProjectiles = GetComponent<PlayerShootProjectiles>();
            _style = GetComponent<Style>();
        }

        private void FixedUpdate()
        {
            controller.Move(_horizontalMovement * Time.fixedDeltaTime, _isCrouchPressed, _isJumpPressed);
            _isJumpPressed = false;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _input = context.ReadValue<Vector2>();
            _horizontalMovement = _input.x * speed;
            if (context.started)
                PlayerMoved?.Invoke();
            else if (context.canceled)
                PlayerIdled?.Invoke();
        }

        public void OnPrimaryShoot(InputAction.CallbackContext context)
        {
            _isPrimaryFirePressed = context.ReadValueAsButton();
            _isCombinedFirePressed = _isPrimaryFirePressed && _isSecondaryFirePressed;

            if (context.started && _isCombinedFirePressed)
            {
                _playerShootProjectiles.Shoot(TypeOfFire.CombinedFire);
                _wasCombinedFirePressed = true;
            }
            else
                switch (context.canceled & !_isSecondaryFirePressed)
                {
                    case true when _wasCombinedFirePressed:
                        _wasCombinedFirePressed = false;
                        break;
                    case true when !_wasCombinedFirePressed:
                        _playerShootProjectiles.Shoot(TypeOfFire.PrimaryFire);
                        break;
                }
        }

        public void OnSecondaryShoot(InputAction.CallbackContext context)
        {
            _isSecondaryFirePressed = context.ReadValueAsButton();
            _isCombinedFirePressed = _isPrimaryFirePressed && _isSecondaryFirePressed;

            if (context.started && _isCombinedFirePressed)
            {
                _playerShootProjectiles.Shoot(TypeOfFire.CombinedFire);
                _wasCombinedFirePressed = true;
            }
            else
                switch (context.canceled && !_isPrimaryFirePressed)
                {
                    case true when _wasCombinedFirePressed:
                        _wasCombinedFirePressed = false;
                        break;
                    case true when !_wasCombinedFirePressed:
                        _playerShootProjectiles.Shoot(TypeOfFire.SecondaryFire);
                        break;
                }
        }

        public void OnFirstStyle(InputAction.CallbackContext context) => _style.CurrentStyle = TypeOfStyle.FirstStyle;
        public void OnSecondStyle(InputAction.CallbackContext context) => _style.CurrentStyle = TypeOfStyle.SecondStyle;
        public void OnThirdStyle(InputAction.CallbackContext context) => _style.CurrentStyle = TypeOfStyle.ThirdStyle;

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
                _isJumpPressed = true;
        }
            

        public void OnCrouch(InputAction.CallbackContext context) => _isCrouchPressed = context.ReadValueAsButton();
    }
}