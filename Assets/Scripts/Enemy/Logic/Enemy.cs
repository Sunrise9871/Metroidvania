using System;
using System.Collections;
using Enemy.EnemyTakingDamageStates;
using GameLogic.Interfaces;
using GameLogic.MainLogic;
using Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Logic
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Tooltip("Количество очков здоровья")]
        [SerializeField] private float maxHealth;
        
        private CapsuleCollider2D _collider;
        
        private EnemyTakingDamageState _enemyTakingDamageState;
        private GameStopScenario _stopScenario;
        private float _health;
        
        public float HealthPercent => _health / maxHealth;

        public event Action Damaged, Died, Healed;
        public event Action<EnemyTakingDamageState> StateChanged;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _stopScenario = FindAnyObjectByType<GameStopScenario>();
            _health = maxHealth;
        }
        
        private void Start() => StartCoroutine(RandomState());

        private void OnEnable() => _stopScenario.GameStopped += OnGameStopped;

        private void OnDisable() => _stopScenario.GameStopped -= OnGameStopped;

        public void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (!_enemyTakingDamageState.IsVulnerable(typeOfFire))
                Heal();
            else if (_health > 1)
                Damage();
            else
                Die();
        }

        private void Die()
        {
            Died?.Invoke();
            _collider.enabled = false;
            enabled = false;
        }

        private void Damage()
        {
            _health--;
            Damaged?.Invoke();
        }

        private void Heal()
        {
            if (_health < maxHealth)
                _health++;
            Healed?.Invoke();
        }

        private IEnumerator RandomState()
        {
            while (enabled)
            {
                var state = Random.Range(0, 3);
                _enemyTakingDamageState = state switch
                {
                    0 => new EnemyTakingDamagePrimaryState(),
                    1 => new EnemyTakingDamageSecondaryState(),
                    2 => new EnemyTakingDamageCombinedState(),
                    _ => _enemyTakingDamageState
                };
                
                StateChanged?.Invoke(_enemyTakingDamageState);
                yield return new WaitForSeconds(6);
            }
        }

        private void OnGameStopped() => enabled = false;
    }
}