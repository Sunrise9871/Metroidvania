using System;
using Shooting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class PlayerInput : MonoBehaviour
    {
        private bool _isPrimaryFirePressed;
        private bool _isSecondaryFirePressed;
        private bool _isCombinedFirePressed;
        private bool _wasCombinedFirePressed;

        public PlayerInputActions PlayerInputActions { get; private set; }

        public event Action<TypeOfFire> Shot;

        private void Awake() => PlayerInputActions = new PlayerInputActions();

        private void OnEnable()
        {
            PlayerInputActions.Enable();

            PlayerInputActions.Player.PrimaryFire.started += OnPrimaryShoot;
            PlayerInputActions.Player.PrimaryFire.canceled += OnPrimaryShoot;

            PlayerInputActions.Player.SecondaryFire.started += OnSecondaryShoot;
            PlayerInputActions.Player.SecondaryFire.canceled += OnSecondaryShoot;
        }

        private void OnDisable() => PlayerInputActions.Disable();

        private void OnPrimaryShoot(InputAction.CallbackContext context)
        {
            _isPrimaryFirePressed = context.ReadValueAsButton();
            _isCombinedFirePressed = _isPrimaryFirePressed && _isSecondaryFirePressed;

            if (context.started && _isCombinedFirePressed)
            {
                Shot?.Invoke(TypeOfFire.CombinedFire);
                _wasCombinedFirePressed = true;
            }
            else if (context.canceled && !_isSecondaryFirePressed)
            {
                if (_wasCombinedFirePressed)
                    _wasCombinedFirePressed = false;
                else
                    Shot?.Invoke(TypeOfFire.PrimaryFire);
            }
        }

        private void OnSecondaryShoot(InputAction.CallbackContext context)
        {
            _isSecondaryFirePressed = context.ReadValueAsButton();
            _isCombinedFirePressed = _isPrimaryFirePressed && _isSecondaryFirePressed;

            if (context.started && _isCombinedFirePressed)
            {
                Shot?.Invoke(TypeOfFire.CombinedFire);
                _wasCombinedFirePressed = true;
            }
            else if (context.canceled && !_isPrimaryFirePressed)
            {
                if (_wasCombinedFirePressed)
                    _wasCombinedFirePressed = false;
                else
                    Shot?.Invoke(TypeOfFire.SecondaryFire);
            }
        }
    }
}