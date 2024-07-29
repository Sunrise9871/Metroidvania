using System.Collections;
using Enemy.EnemyTakingDamageStates;
using Enemy.Logic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemy.Graphics
{
    [RequireComponent(typeof(Logic.Enemy))]
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Light2D))]
    public class EnemyGraphics : MonoBehaviour
    {
        private const float DissolveTimeScale = 0.5f;
        
        #region AnimatorHash

        private readonly int _jumpProgress = Animator.StringToHash("JumpProgress");
        private readonly int _deadTrigger = Animator.StringToHash("Dead");
        
        #endregion
        
        private readonly Color _damageColor = Color.red;
        private readonly Color _healColor = Color.green;
        private readonly Color _defaultColor = Color.white;
        private readonly WaitForSeconds _healthChangeEffectTime = new (0.3f);
        private readonly int _fadeShader = Shader.PropertyToID("_Fade");
        
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Light2D _light2D;
        
        private Logic.Enemy _enemy;
        private EnemyMovement _enemyMovement;
        private Coroutine _coroutine;

        private void Awake()
        {
            _enemy = GetComponent<Logic.Enemy>();
            _enemyMovement = GetComponent<EnemyMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _light2D = GetComponent<Light2D>();
        }

        private void OnEnable()
        {
            _enemy.StateChanged += OnStateChanged;

            _enemy.Damaged += OnDamaged;
            _enemy.Healed += OnHealed;
            _enemy.Died += OnDied;
            
            _enemyMovement.NewDestinationSet += SetSpriteSide;
            _enemyMovement.JumpProgressChanged += OnJumpProgressChanged;
        }

        private void OnDisable()
        {
            _enemy.StateChanged -= OnStateChanged;
            
            _enemy.Damaged -= OnDamaged;
            _enemy.Healed -= OnHealed;
            _enemy.Died -= OnDied;
            
            _enemyMovement.NewDestinationSet -= SetSpriteSide;
            _enemyMovement.JumpProgressChanged -= OnJumpProgressChanged;
        }

        private void SetSpriteSide(Transform destination) =>
            _spriteRenderer.flipX = destination.position.x < transform.position.x;

        private void OnStateChanged(EnemyTakingDamageState state) => _light2D.color = state.ColorMark;

        private void OnJumpProgressChanged(float progress) => _animator.SetFloat(_jumpProgress, progress);

        private void OnDamaged() => PlayHealthChangeEffect(_damageColor);

        private void OnHealed() => PlayHealthChangeEffect(_healColor);

        private void OnDied()
        {
            _animator.SetTrigger(_deadTrigger);
            StartCoroutine(DissolveSprite());
        } 

        private void PlayHealthChangeEffect(Color color) 
        {
            if (_coroutine is not null)
                StopCoroutine(_coroutine);    
            
            _coroutine = StartCoroutine(Coroutine());
            return;
            
            IEnumerator Coroutine()
            {
                _spriteRenderer.color = color;
                yield return _healthChangeEffectTime;
                _spriteRenderer.color = _defaultColor;
            }
        }

        private IEnumerator DissolveSprite()
        {
            var material = _spriteRenderer.material;
            
            for (var i = 1f; i > 0f; i -= Time.deltaTime * DissolveTimeScale)
            {
                print(i);
                material.SetFloat(_fadeShader, i);
                yield return null;
            }
        }
    }
}