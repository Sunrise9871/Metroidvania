using System;
using Shooting;
using UnityEngine;

namespace Player.Movement
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
        
        private UnityEngine.Camera _camera;
        private PlayerInput _playerInput;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            _primaryPool = new BulletSpawner(pfPrimaryBullet);
            _secondaryPool = new BulletSpawner(pfSecondaryBullet);
            _combinedPool = new BulletSpawner(pfCombinedBullet);
            _camera = UnityEngine.Camera.main;
        }

        private void OnEnable() => _playerInput.PlayerShot += OnShot;

        private void OnDisable() => _playerInput.PlayerShot -= OnShot;

        /// <summary>
        ///   <para>Выстреливает с помощью projectile.</para>
        /// </summary>
        private void OnShot(TypeOfFire typeOfFire)
        {
            // Позиция курсора в мировых координатах
            var cursorPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            // Обнулить z координату
            cursorPosition.z = transform.position.z;
            // Вычислить направление от позиции игрока к позиции курсора
            var direction = cursorPosition - transform.position;
            // Вычисление угла в градусах
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
                () => OnReleaseBullet(bulletScript));
        }
        
        /// <summary>
        /// Обработчик события для возвращения projectile в pool
        /// </summary>
        /// <param name="bulletScript">Скрипт projectile</param>
        private void OnReleaseBullet(Bullet bulletScript) => bulletScript.GetPool().Release(bulletScript);
    }
}