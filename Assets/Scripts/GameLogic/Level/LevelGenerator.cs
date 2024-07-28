using System;
using System.Collections.Generic;
using Enemy.Logic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameLogic.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        private const float CheckLevelRepeatTime = 0.5f;
        private const float MinDistanceToSpawnPlatform = 80f;
        private const float RedZoneDifference = 50f;
        
        [Tooltip("Список платформ для генерации уровня")]
        [SerializeField] private List<GameObject> platforms;

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

        private Queue<Transform> _activePlatformsQueue;

        public event Action<Transform> PlatformSpawned;

        private void Awake()
        {
            _activePlatformsQueue = new Queue<Transform>();
            InvokeRepeating(nameof(ManageLevel), 0f, CheckLevelRepeatTime);
        }

        private void OnEnable() => enemy.NextPlatformNotFound += SpawnPlatform;

        private void OnDisable() => enemy.NextPlatformNotFound -= SpawnPlatform;

        private void ManageLevel()
        {
            while (_activePlatformsQueue.TryPeek(out var lowestPlatform))
                if (lowestPlatform.position.y < redZone.transform.position.y - RedZoneDifference)
                    DestroyLowestPlatform();
                else break;

            if (!lastPlatform) return;
            while (lastPlatform.position.y < player.position.y + MinDistanceToSpawnPlatform)
                SpawnPlatform();
        }

        private void DestroyLowestPlatform()
        {
            var platform = _activePlatformsQueue.Dequeue().gameObject;
            Destroy(platform);
        } 
        
        private void SpawnPlatform()
        {
            var newPosition = new Vector3(
                Random.Range(minX, maxX),
                lastPlatform.position.y + Random.Range(minY, maxY),
                lastPlatform.position.z);
            var randomPlatformPrefab = platforms[Random.Range(0, platforms.Count)];
            
            lastPlatform = Instantiate(randomPlatformPrefab, newPosition, Quaternion.identity, transform).transform;
            _activePlatformsQueue.Enqueue(lastPlatform);

            PlatformSpawned?.Invoke(lastPlatform);
        }
    }
}