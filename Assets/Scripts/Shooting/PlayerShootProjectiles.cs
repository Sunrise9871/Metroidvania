using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooting
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private Transform pfBullet;

        public void Shoot()
        {
            //print(transform.localScale);
            if (!Camera.main) return;
            
            // Получаем позицию курсора в мировых координатах
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = transform.position.z; // Обнуляем z координату для 2D игр

            // Вычисляем направление от позиции игрока к позиции курсора
            var direction = cursorPosition - transform.position;

            // Определяем угол в градусах, используя арктангенс
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Создаем кватернион на основе угла
            var rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
            var bulletTransform = Instantiate(pfBullet, transform.position, rotation);
            bulletTransform.GetComponent<Bullet>().Setup(direction.normalized);
        }
    }
}
