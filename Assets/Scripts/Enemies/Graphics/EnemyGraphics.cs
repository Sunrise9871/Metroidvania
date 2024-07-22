using Enemies.EnemyStates;
using Enemies.Logic;
using UnityEngine;

namespace Enemies.Graphics
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(Animator))]
    public class EnemyGraphics : MonoBehaviour
    {
        #region AnimatorHash

        private readonly int _jumpProgress = Animator.StringToHash("JumpProgress");
        
        #endregion
        
        private Enemy _enemy;
        private EnemyMovement _enemyMovement;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _enemyMovement = GetComponent<EnemyMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _enemy.StateChanged += OnStateChanged;
            _enemyMovement.NewDestinationSet += SetSpriteSide;
            
            _enemyMovement.JumpProgressChanged += OnJumpProgressChanged;
        }

        private void OnDisable()
        {
            _enemy.StateChanged -= OnStateChanged;
            _enemyMovement.NewDestinationSet -= SetSpriteSide;
            
            _enemyMovement.JumpProgressChanged -= OnJumpProgressChanged;
        }

        private void SetSpriteSide(Transform destination) =>
            _spriteRenderer.flipX = destination.position.x < transform.position.x;

        private void OnStateChanged(EnemyTakingDamageState state)
        {
        }

        private void OnJumpProgressChanged(float progress) => _animator.SetFloat(_jumpProgress, progress);
    }
}