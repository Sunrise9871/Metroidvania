﻿using System.Collections;
using Enemy.ShotStyles;
using Shooting;
using UnityEngine;

namespace Enemy.Logic
{
    public class EnemyShooting : MonoBehaviour
    {
        [Tooltip("Префаб для projectile")]
        [SerializeField] private GameObject pfBullet;

        [Tooltip("Трансформ игрока - цель для стрельбы")]
        [SerializeField] private Transform player;

        [Tooltip("Частота стрельбы в секундах")]
        [SerializeField] private float shootingFrequency;

        private BulletSpawner _bulletPool;
        private ShotStyle _shotStyle;

        private void Start()
        {
            _shotStyle = new TripleShotStyle();
            _bulletPool = new BulletSpawner(pfBullet);
            StartCoroutine(nameof(Shoot));
        }

        private IEnumerator Shoot()
        {
            var awaitTime = new WaitForSeconds(shootingFrequency);
            
            while (true)
            {
                var direction = (player.position - transform.position).normalized;

                var shotStyle = _shotStyle.GetGeometry(direction);

                for (var i = 0; i < shotStyle.Directions.Count; i++)
                {
                    var bulletScript = _bulletPool.Get();
                    bulletScript.Setup(transform.position, shotStyle.Rotations[i], shotStyle.Directions[i],
                        () => _bulletPool.Release(bulletScript));
                }
                
                yield return awaitTime;
            }
        }
        
    }
}