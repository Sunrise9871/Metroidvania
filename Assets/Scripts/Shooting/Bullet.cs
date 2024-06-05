using System;
using UnityEngine;

namespace Shooting
{
    public class Bullet : MonoBehaviour
    {
        [Tooltip("Объект с particle system, создающийся при попадании.")] [SerializeField]
        private GameObject particleSystemGameObject;

        [Tooltip("Скорость полета projectile.")] [SerializeField]
        private float moveSpeed = 10f;

        [Tooltip("Время до уничтожения projectile в секундах.")] [SerializeField]
        private float timeToDestroy = 5f;

        private Action _onReachTarget;

        private Rigidbody2D _rb;

        /// <summary>
        /// <para>Устанавливает направление движения projectile и запускает его.</para>
        /// </summary>
        /// <param name="transformPosition">Позиция создания</param>
        /// <param name="transformRotation">Поворот во время создания</param>
        /// <param name="shootDirection">Направление движения/</param>
        /// <param name="onReachTarget"></param>
        public void Setup(Vector3 transformPosition, Quaternion transformRotation, Vector3 shootDirection,
            Action onReachTarget)
        {
            transform.position = transformPosition;
            transform.rotation = transformRotation;

            _rb = GetComponent<Rigidbody2D>();
            _rb.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);

            _onReachTarget = onReachTarget;

            //TODO: pool
            //Уничтожение по таймеру
            //Destroy(gameObject, timeToDestroy);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Проверка попадания в цель
            var target = other.GetComponent<Target>();
            if (!target) return;

            //Нанесение урона цели
            target.Damage();

            //Создание партиклов
            //TODO: POOL
            var particles = Instantiate(particleSystemGameObject, gameObject.transform);
            particles.transform.SetParent(transform.parent, true);

            //Уничтожение projectile
            _onReachTarget();
        }
    }
}