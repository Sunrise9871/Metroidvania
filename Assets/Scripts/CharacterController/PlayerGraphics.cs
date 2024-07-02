using UnityEngine;

namespace CharacterController
{
    public class PlayerGraphics : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private Animator _animator;
        
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _idle = Animator.StringToHash("Idle");

        private void Awake()
        {
            _playerMovement = GetComponentInParent<PlayerMovement>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _playerMovement.PlayerMoved += OnMove;
            _playerMovement.PlayerIdled += OnIdle;
        }

        private void OnDisable()
        {
            _playerMovement.PlayerMoved -= OnMove;
            _playerMovement.PlayerIdled -= OnIdle;
        }

        private void OnMove()
        {
            _animator.SetTrigger(_move);
        }

        private void OnIdle() => _animator.SetTrigger(_idle);
    }
}