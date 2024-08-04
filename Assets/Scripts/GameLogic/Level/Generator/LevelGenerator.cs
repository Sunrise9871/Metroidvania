using System;
using System.Collections.Generic;
using Enemy.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.Level.Generator
{
    public class LevelGenerator : MonoBehaviour
    {
        private const float CheckLevelRepeatTime = 0.5f;
        private const float MinDistanceToSpawnPlatform = 80f;
        private const float RedZoneDifference = 50f;
        
        private readonly Queue<Transform> _activePlatforms = new();
        
        [Tooltip("Платформа для генерации уровня")]
        [SerializeField] private Transform platform;

        [Tooltip("Трансформ игрока")]
        [SerializeField] private Transform player;

        [Tooltip("Враг")]
        [SerializeField] private EnemyMovement enemy;

        [Tooltip("Уничтожающая платформа")]
        [SerializeField] private Transform redZone;

        [Tooltip("Платформа, от которой генерируется следующая платформа")]
        [SerializeField] private Transform lastPlatform;

        [Tooltip("Параметры рандома генерации")]
        [SerializeField] private int minX, maxX, minY, maxY;

        private PlatformSpawner _platformSpawner;

        public event Action<Transform> PlatformSpawned;

        private void Awake()
        {
            _platformSpawner = new PlatformSpawner(platform);
            InvokeRepeating(nameof(ManageLevel), 0f, CheckLevelRepeatTime);
        }

        private void OnEnable() => enemy.NextPlatformNotFound += SpawnPlatform;

        private void OnDisable() => enemy.NextPlatformNotFound -= SpawnPlatform;

        private void ManageLevel()
        {
            while (_activePlatforms.TryPeek(out var lowestPlatform))
                if (lowestPlatform.position.y < redZone.transform.position.y - RedZoneDifference)
                    DestroyLowestPlatform();
                else break;

            if (!lastPlatform) return;
            while (lastPlatform.position.y < player.position.y + MinDistanceToSpawnPlatform)
                SpawnPlatform();
        }

        private void DestroyLowestPlatform()
        {
            var lowestPlatform = _activePlatforms.Dequeue();
            _platformSpawner.Release(lowestPlatform);
        }

        private void SpawnPlatform()
        {
            var newPosition = new Vector3(
                Random.Range(minX, maxX),
                lastPlatform.position.y + Random.Range(minY, maxY),
                lastPlatform.position.z);
            
            lastPlatform = _platformSpawner.Get();
            lastPlatform.position = newPosition;
            lastPlatform.parent = transform;
            
            _activePlatforms.Enqueue(lastPlatform);
            PlatformSpawned?.Invoke(lastPlatform);
        }
    }
}