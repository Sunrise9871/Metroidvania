using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shooting
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Bullet))]
    public class BulletGraphics : MonoBehaviour
    {
        private Bullet _bullet;

        private ParticleSystem _particleSystemGameObject;
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;
        private Animator _animator;

        private void Awake()
        {
            _bullet = GetComponent<Bullet>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _particleSystemGameObject = GetComponent<ParticleSystem>();
            _light = GetComponent<Light2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _bullet.BulletExploded += OnBulletExploded;

            _spriteRenderer.enabled = true;
            if (_light) _light.enabled = true;
            _animator.enabled = true;
        }

        private void OnDisable() => _bullet.BulletExploded -= OnBulletExploded;

        private void OnBulletExploded()
        {
            _spriteRenderer.enabled = false;
            if (_light) _light.enabled = false;
            _animator.enabled = false;

            _particleSystemGameObject.Play();
        }
    }
}