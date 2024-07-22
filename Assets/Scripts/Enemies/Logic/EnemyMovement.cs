using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.Level;
using UnityEngine;

namespace Enemies.Logic
{
    public class EnemyMovement : MonoBehaviour
    {
        private const float MaximumAcceleration = 2f;
        private const float MinimumAcceleration = 0.5f;

        [Tooltip("Генератор уровня")]
        [SerializeField] private LevelGenerator levelGenerator;
        
        [Tooltip("Первое место назначения")]
        [SerializeField] private Transform firstDestination;

        [Tooltip("Ноги персонажа (для точных приземлений)")]
        [SerializeField] private Transform feet;

        [Tooltip("Transform игрока")]
        [SerializeField] private Transform player;

        [Tooltip("Новое место назначения для прыжка выше текущего не менее, чем на ...")]
        [SerializeField] private float minHeightForNewDestination = 8f;

        [Tooltip("Сила эффекта параболического прыжка")]
        [SerializeField] private float jumpHeight = 5f;

        [Tooltip("Длительность прыжка")]
        [SerializeField] private float jumpDuration = 0.5f;

        [Tooltip("Нормальная скорость при расстоянии от игрока в...")]
        [SerializeField] private float normalDistance = 15f;

        private Queue<Transform> _paths;
        
        public event Action<Transform> NewDestinationSet;
        public event Action NextPlatformNotFound;

        private void Awake() => _paths = new Queue<Transform>();

        private void OnEnable() => levelGenerator.PlatformSpawned += AddNewSpawnedPlatform;

        private void OnDisable() => levelGenerator.PlatformSpawned -= AddNewSpawnedPlatform;

        private void Start() => StartCoroutine(Jump(firstDestination));
        
        private void AddNewSpawnedPlatform(Transform spawnedPlatform) => _paths.Enqueue(spawnedPlatform);
        
        private void SetNewDestinationFromQueue()
        {
            while (true)
            {
                while (_paths.TryDequeue(out var destination))
                {
                    if (destination.position.y - transform.position.y < minHeightForNewDestination) continue;
                    NewDestinationSet?.Invoke(destination);
                    StartCoroutine(Jump(destination));
                    return;
                }

                NextPlatformNotFound?.Invoke();
            }
        }
        
        private float CalculateAcceleration()
        {
            var acceleration = normalDistance / (transform.position.y - player.position.y);
            return acceleration < 0
                ? MaximumAcceleration
                : Mathf.Clamp(acceleration, MinimumAcceleration, MaximumAcceleration);
        }
        
        private IEnumerator Jump(Transform destination)
        {
            var startPosition = transform.position;
            var adjustedJumpDuration = jumpDuration / CalculateAcceleration();

            for (float jumpTime = 0; jumpTime < adjustedJumpDuration; jumpTime += Time.deltaTime)
            {
                var jumpProgress = jumpTime / adjustedJumpDuration;
                var height = Mathf.Sin(Mathf.PI * jumpProgress) * jumpHeight;
                transform.position = Vector3.Lerp(startPosition, destination.position, jumpProgress)
                                     + new Vector3(0, height, 0);
                yield return null;
            }

            SetNewDestinationFromQueue();
        }
    }
}