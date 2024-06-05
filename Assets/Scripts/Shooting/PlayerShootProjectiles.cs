using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Shooting
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private GameObject pfPrimaryBullet;
        [SerializeField] private GameObject pfSecondaryBullet;
        [SerializeField] private GameObject pfCombinedBullet;

        private CustomUnityPool.CustomUnityPool _primaryPool;
        private CustomUnityPool.CustomUnityPool _secondaryPool;
        private CustomUnityPool.CustomUnityPool _combinedPool;

        private void Awake()
        {
            _primaryPool = new CustomUnityPool.CustomUnityPool(pfPrimaryBullet, 10);
            _secondaryPool = new CustomUnityPool.CustomUnityPool(pfSecondaryBullet, 10);
            _combinedPool = new CustomUnityPool.CustomUnityPool(pfCombinedBullet, 10);
        }

        /// <summary>
        /// Выстреливает с помощью projectile.
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
            
            var bulletGameObject = typeOfFire switch
            {
                TypeOfFire.PrimaryFire => _primaryPool.Get(),
                TypeOfFire.SecondaryFire => _secondaryPool.Get(),
                TypeOfFire.CombinedFire => _combinedPool.Get(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeOfFire), typeOfFire, null)
            };
            
            
            bulletGameObject.GetComponent<Bullet>().Setup(transform.position, rotation, direction.normalized,
                OnReachTarget);

            void OnReachTarget()
            {
                switch (typeOfFire)
                {
                    case TypeOfFire.PrimaryFire:
                        _primaryPool.Release(bulletGameObject);
                        break;
                    case TypeOfFire.SecondaryFire:
                        _secondaryPool.Release(bulletGameObject);
                        break;
                    case TypeOfFire.CombinedFire:
                        _combinedPool.Release(bulletGameObject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(typeOfFire), typeOfFire, null);
                }
            }
            
        }
    }
}
