using System.Collections;
using Enemy.EnemyStages;
using GameLogic.MainLogic;
using Shooting;
using UnityEngine;

namespace Enemy.Logic
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyShooting : MonoBehaviour
    {
        private const float NormalHealth = 0.75f;
        private const float LowHealth = 0.55f;
        private const float CriticalHealth = 0.3f;
        
        private readonly EasyEnemyStage _easyEnemyStage = new();
        private readonly NormalEnemyStage _normalEnemyStage = new();
        private readonly HardEnemyStage _hardEnemyStage = new();
        private readonly RageEnemyStage _rageEnemyStage = new();

        [Tooltip("Префаб для projectile")]
        [SerializeField] private GameObject pfBullet;

        [Tooltip("Трансформ игрока - цель для стрельбы")]
        [SerializeField] private Transform player;

        private BulletSpawner _bulletPool;
        private EnemyStage _enemyStage;
        private Enemy _enemy;
        private GameStopScenario _stopScenario;

        private void Awake()
        {
            _enemyStage = _easyEnemyStage;
            _bulletPool = new BulletSpawner(pfBullet);
            _enemy = GetComponent<Enemy>();
            _stopScenario = FindAnyObjectByType<GameStopScenario>();
        }

        private void Start() => StartCoroutine(nameof(Shoot));

        private void OnEnable()
        {
            _enemy.Died += OnGameStopped;
            _enemy.Damaged += OnDamaged;
            _stopScenario.GameStopped += OnGameStopped;
        }

        private void OnDisable()
        {
            _enemy.Died -= OnGameStopped;
            _enemy.Damaged -= OnDamaged;
            _stopScenario.GameStopped -= OnGameStopped;
        }

        private IEnumerator Shoot()
        {
            while (enabled)
            {
                var direction = (player.position - transform.position).normalized;

                var shotStyle = _enemyStage.ShootingStyle.GetGeometry(direction);

                for (var i = 0; i < shotStyle.Directions.Count; i++)
                {
                    var bulletScript = _bulletPool.Get();
                    bulletScript.Setup(transform.position, shotStyle.Rotations[i], shotStyle.Directions[i],
                        () => _bulletPool.Release(bulletScript));
                }

                yield return new WaitForSeconds(_enemyStage.ShootSpeed);
            }
        }

        private void OnDamaged()
        {
            _enemyStage = _enemy.HealthPercent switch
            {
                > NormalHealth => _easyEnemyStage,
                > LowHealth => _normalEnemyStage,
                > CriticalHealth => _hardEnemyStage,
                < CriticalHealth => _rageEnemyStage,
                _ => _enemyStage
            };
        }
        
        private void OnGameStopped() => enabled = false;
    }
}