using System.Collections;
using CustomUnityPools;
using Shooting.Bullets;
using UnityEngine;

namespace Enemies
{
    public class EnemyShooting : MonoBehaviour
    {
        [Tooltip("Преваб для projectile")]
        [SerializeField] private GameObject pfBullet;

        [Tooltip("Трансформ игрока - цель для стрельбы")]
        [SerializeField] private Transform player;

        [Tooltip("Частота стрельбы в секундах")]
        [SerializeField] private float shootingFrequency;

        private BulletSpawner _bulletPool; // Object pool

        private void Start()
        {
            _bulletPool = new BulletSpawner(pfBullet);
        }

        private IEnumerator Shoot()
        {
            // Вычислить направление от позиции врага к позиции игрока
            var direction = player.position - transform.position;
            // Угол в градусах
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
            var bulletScript = _bulletPool.Get();
            
            //Инициализация снаряда
            bulletScript.Setup(transform.position, rotation, direction.normalized,
                () => OnReleaseBullet(bulletScript));
            yield return new WaitForSeconds(shootingFrequency);
        }
        
        /// <summary>
        /// Обработчик события для возвращения projectile в pool
        /// </summary>
        /// <param name="bulletScript">Скрипт projectile</param>
        private void OnReleaseBullet(Bullet bulletScript) => _bulletPool.Release(bulletScript);
    }
}