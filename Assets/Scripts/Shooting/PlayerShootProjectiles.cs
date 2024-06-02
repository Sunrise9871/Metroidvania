using System;
using UnityEngine;

namespace Shooting
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private Transform pfPrimaryBullet;
        [SerializeField] private Transform pfSecondaryBullet;
        [SerializeField] private Transform pfCombinedBullet;

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

            var bulletTransform = typeOfFire switch
            {
                TypeOfFire.PrimaryFire => Instantiate(pfPrimaryBullet, transform.position, rotation),
                TypeOfFire.SecondaryFire => Instantiate(pfSecondaryBullet, transform.position, rotation),
                TypeOfFire.CombinedFire => Instantiate(pfCombinedBullet, transform.position, rotation),
                _ => throw new ArgumentOutOfRangeException(nameof(typeOfFire), typeOfFire, null)
            };

            bulletTransform?.GetComponent<Bullet>().Setup(direction.normalized);
        }
    }
}
