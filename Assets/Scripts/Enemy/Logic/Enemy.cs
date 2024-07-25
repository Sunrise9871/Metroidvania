using System;
using System.Collections;
using Enemy.EnemyTakingDamageStates;
using GameLogic.Interfaces;
using Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Logic
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Tooltip("Количество очков здоровья")]
        [SerializeField] private float maxHealth;
        
        private float _health;
        private EnemyTakingDamageState _enemyTakingDamageState;

        public float HealthPercent => _health / maxHealth;

        public event Action Damaged, Died, Healed;
 
        public event Action<EnemyTakingDamageState> StateChanged;

        private void Start()
        {
            _health = maxHealth;
            StartCoroutine(RandomState());
        }

        public void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (!_enemyTakingDamageState.IsVulnerable(typeOfFire))
            {
                if (_health < maxHealth)
                    _health++;
                Healed?.Invoke();
            }
            else if (_health > 1)
            {
                _health--;
                Damaged?.Invoke();
            }
            else
            {
                Died?.Invoke();
                //Destroy(gameObject);
            }
                
        }

        private IEnumerator RandomState()
        {
            while (true)
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
    }
}