using System;
using GameLogic.Interfaces;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shooting
{
    /// <summary>
    ///   <para>Класс для projectile снарядов, используемые игроком.</para>
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Bullet : MonoBehaviour
    {
        [Tooltip("Скорость полета projectile.")]
        [SerializeField] private float moveSpeed = 10f;

        [Tooltip("Время до уничтожения projectile в секундах.")]
        [SerializeField] private float timeToDestroy = 5f;

        [Tooltip("Тип огня.")]
        [SerializeField] private TypeOfFire typeOfFire;

        private BulletSpawner _pool; //Ссылка на object pool
        private Action _releaseAction; //Локальная функция с действиями при возврате в object pool

        #region Компоненты для OnDisable/OnEnable

        private ParticleSystem _particleSystemGameObject;
        private BoxCollider2D _collider;
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;
        private Animator _animator;

        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _particleSystemGameObject = GetComponent<ParticleSystem>();
            _light = GetComponent<Light2D>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        ///   <para>Устанавливает направление движения projectile и запускает его.</para>
        /// </summary>
        /// <param name="transformPosition">Позиция создания</param>
        /// <param name="transformRotation">Поворот во время создания</param>
        /// <param name="shootDirection">Направление движения/</param>
        /// <param name="onReleaseBullet">Локальная функция с действиями после возврата в object pool</param>
        public void Setup(Vector3 transformPosition, Quaternion transformRotation, Vector3 shootDirection,
            Action onReleaseBullet)
        {
            //Задание позиции
            transform.position = transformPosition;
            transform.rotation = transformRotation;

            //Придание силы по направлению
            _rb.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);

            //Локальная функция с действиями при возврате в object pool
            _releaseAction = onReleaseBullet;

            //Уничтожение по таймеру
            Invoke(nameof(HideProjectile), timeToDestroy);
        }

        /// <summary>
        ///   <para>Указывает ссылку на object pool для объекта.</para>
        /// </summary>
        /// <param name="pool">Ссылка на object pool</param>
        public void SetPool(BulletSpawner pool) => _pool = pool;

        /// <summary>
        ///   <para>Возвращает ссылку на object pool для объекта.</para>
        /// </summary>
        /// <returns>Ссылка на object pool</returns>
        public BulletSpawner GetPool() => _pool;

        /// <summary>
        ///   <para>Вызывается при вызове из object pool.</para>
        /// </summary>
        private void OnEnable()
        {
            gameObject.isStatic = false;
            _collider.enabled = true;
            _spriteRenderer.enabled = true;
            if (_light) _light.enabled = true;
            _animator.enabled = true;
        }

        /// <summary>
        ///   <para>Возвращает projectile в object pool.</para>
        /// </summary>
        private void ReleaseProjectile() => _releaseAction();

        /// <summary>
        ///   <para>Действия при столкновении с объектом.</para>
        /// </summary>
        /// <param name="other">Объект, с которым столкнулись</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Проверка попадания в цель
            var target = other.GetComponent<IDamageable>();
            SendDamage(target);
            CancelInvoke(nameof(HideProjectile)); //Отмена уничтожения projectile по таймауту
            HideProjectile();
        }

        private void HideProjectile()
        {
            //Скрытие projectile
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
            if (_light) _light.enabled = false;
            _rb.Sleep();
            _animator.enabled = false;

            //Запуск particle system
            _particleSystemGameObject.Play();
            gameObject.isStatic = true;
        }

        /// <summary>
        ///   <para>Отправляет тип projectile скрипту цели</para>
        /// </summary>
        /// <param name="target"></param>
        private void SendDamage(IDamageable target) => target.ReceiveDamage(typeOfFire);

        /// <summary>
        ///   <para>Когда заканчивается анимация particle system, projectile возвращается в object pool.</para>
        /// </summary>
        private void OnParticleSystemStopped() => ReleaseProjectile();
    }
}