using System;
using GameLogic.Interfaces;
using UnityEngine;

namespace Shooting
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Bullet : MonoBehaviour
    {
        [Tooltip("Скорость полета projectile")]
        [SerializeField] private float moveSpeed = 10f;

        [Tooltip("Время до уничтожения projectile в секундах")]
        [SerializeField] private float timeToDestroy = 5f;

        [Tooltip("Тип огня")]
        [SerializeField] private TypeOfFire typeOfFire;

        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _collider;
        
        private Action _releaseAction;

        public event Action BulletExploded;

        public void Setup(Vector3 spawnPosition, Quaternion spawnRotation, Vector3 shootDirection,
            Action onReleaseBullet)
        {
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
            _rigidbody2D.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);
            _releaseAction = onReleaseBullet;
            
            Invoke(nameof(ExplodeBullet), timeToDestroy);
        }
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            gameObject.AddComponent<BulletGraphics>();
        }
        
        private void OnEnable()
        {
            _collider.enabled = true;
            _rigidbody2D.WakeUp();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponent<IDamageable>();
            if (target is null) return;
            
            target.ReceiveDamage(typeOfFire);

            CancelInvoke(nameof(ExplodeBullet));
            ExplodeBullet();
        }

        private void ExplodeBullet()
        {
            _collider.enabled = false;
            _rigidbody2D.Sleep();
            
            BulletExploded?.Invoke();
        }
        
        private void OnParticleSystemStopped() => _releaseAction();
    }
}