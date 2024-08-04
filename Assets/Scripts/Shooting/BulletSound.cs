using UnityEngine;

namespace Shooting
{
    [RequireComponent(typeof(Bullet))]
    [RequireComponent(typeof(AudioSource))]
    public class BulletSound : MonoBehaviour
    {
        [Tooltip("Звук создания projectile")]
        [SerializeField] private AudioClip creationSound;

        [Tooltip("Звук взрыва projectile")]
        [SerializeField] private AudioClip explosionSound;

        private AudioSource _audioSource;
        private Bullet _bullet;

        private void Awake()
        {
            _bullet = GetComponent<Bullet>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _bullet.BulletExploded += OnExploded;
            
            _audioSource.PlayOneShot(creationSound);
        } 
        
        private void OnDisable() => _bullet.BulletExploded -= OnExploded;

        private void OnExploded() => _audioSource.PlayOneShot(explosionSound);
    }
}