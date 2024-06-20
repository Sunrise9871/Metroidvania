using System.Collections;
using Enemies.EnemyStates;
using Shooting;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private EnemyState _enemyState;

        private void Start() => StartCoroutine(RandomState());

        public void ReceiveDamage(TypeOfFire typeOfFire) => _enemyState.ReceiveDamage(typeOfFire);

        private IEnumerator RandomState()
        {
            while (true)
            {
                var state = Random.Range(0, 4);
                _enemyState = state switch
                {
                    0 => new EnemyPrimaryState(),
                    1 => new EnemySecondaryState(),
                    2 => new EnemyCombinedState(),
                    _ => _enemyState
                };
                print(_enemyState);
                yield return new WaitForSeconds(6);
            }
        }
    }
}