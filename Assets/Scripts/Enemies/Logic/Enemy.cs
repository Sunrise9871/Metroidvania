using System;
using System.Collections;
using Enemies.EnemyStates;
using GameLogic.Interfaces;
using Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.Logic
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Tooltip("Количество очков здоровья")]
        [SerializeField] private float health;

        private EnemyTakingDamageState _enemyTakingDamageState;

        public event Action<EnemyTakingDamageState> StateChanged;

        private void Start() => StartCoroutine(RandomState());

        public void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (!_enemyTakingDamageState.IsVulnerable(typeOfFire))
                health++;
            else if (health > 1)
                health--;
            else
                Destroy(gameObject);
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