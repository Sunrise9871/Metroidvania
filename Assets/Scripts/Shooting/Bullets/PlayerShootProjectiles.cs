using System;
using CustomUnityPools;
using UnityEngine;

namespace Shooting
{
    /// <summary>
    ///   <para>Класс, отвечающий за запуск projectile от игрока.</para>
    /// </summary>
    public class PlayerShootProjectiles : MonoBehaviour
    {
        //Префабы с projectile
        [SerializeField] private GameObject pfPrimaryBullet;
        [SerializeField] private GameObject pfSecondaryBullet;
        [SerializeField] private GameObject pfCombinedBullet;
        
        //Object pool-ы для префабов
        private BulletSpawner _primaryPool;
        private BulletSpawner _secondaryPool;
        private BulletSpawner _combinedPool;
        
        private void Awake()
        {
            _primaryPool = new BulletSpawner(pfPrimaryBullet);
            _secondaryPool = new BulletSpawner(pfSecondaryBullet);
            _combinedPool = new BulletSpawner(pfCombinedBullet);
        }
        
        /// <summary>
        ///   <para>Выстреливает с помощью projectile.</para>
        /// </summary>
        public void Shoot(TypeOfFire typeOfFire)
        {
            if (!Camera.main) return;

            // Позиция курсора в мировых координатах
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Обнулить z координату
            cursorPosition.z = transform.position.z;
            // Вычислить направление от позиции игрока к позиции курсора
            var direction = cursorPosition - transform.position;
            // Угол в градусах
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //Определеть какой снаряд нужен
            var bulletScript = typeOfFire switch
            {
                TypeOfFire.PrimaryFire => _primaryPool.Get(),
                TypeOfFire.SecondaryFire => _secondaryPool.Get(),
                TypeOfFire.CombinedFire => _combinedPool.Get(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeOfFire), typeOfFire, null)
            };
            
            //Инициализация снаряда
            bulletScript.Setup(transform.position, rotation, direction.normalized,
                OnReleaseBullet);
            return;

            //Возвращение projectile в свой object pool
            void OnReleaseBullet() => bulletScript.GetPool().Release(bulletScript);
        }
    }
}