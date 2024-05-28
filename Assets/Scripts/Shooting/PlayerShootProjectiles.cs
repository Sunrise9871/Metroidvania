using UnityEngine;

namespace Shooting
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private Transform pfBullet;

        /// <summary>
        /// Выстреливает с помощью projectile.
        /// </summary>
        public void Shoot()
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
            var bulletTransform = Instantiate(pfBullet, transform.position, rotation);
            bulletTransform.GetComponent<Bullet>().Setup(direction.normalized);
        }
    }
}
