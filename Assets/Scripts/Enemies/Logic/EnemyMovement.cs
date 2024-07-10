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

        private Queue<Transform> _paths; // Очередь путей для врага

        /// <summary>
        ///   <para>Враг получил новое место назначение.</para>
        /// </summary>
        public event Action<Transform> NewDestinationSet;

        private void Awake() => _paths = new Queue<Transform>();

        private void OnEnable() => LevelGenerator.OnPlatformSpawned += AddNewSpawnedPlatform;

        private void OnDisable() => LevelGenerator.OnPlatformSpawned -= AddNewSpawnedPlatform;

        private void Start() => StartCoroutine(Jump(firstDestination));

        /// <summary>
        ///   <para>Добавляет новую созданную на уровне платформу в очередь.</para>
        /// </summary>
        private void AddNewSpawnedPlatform(Transform spawnedPlatform) => _paths.Enqueue(spawnedPlatform);

        /// <summary>
        ///   <para>Выбирает из очереди и устанавливает новое место назначение для врага.</para>
        /// </summary>
        private void SetNewDestinationFromQueue()
        {
            while (_paths.TryDequeue(out var destination))
            {
                if (destination.position.y - transform.position.y < minHeightForNewDestination) continue;
                NewDestinationSet?.Invoke(destination);
                StartCoroutine(Jump(destination));
                return;
            }
        }

        /// <summary>
        ///   <para>Возвращает модификатор скорости для врага на основе расстояния до игрока.</para>
        /// </summary>
        private float CalculateAcceleration()
        {
            var acceleration = normalDistance / (transform.position.y - player.position.y);
            return acceleration < 0
                ? MaximumAcceleration
                : Mathf.Clamp(acceleration, MinimumAcceleration, MaximumAcceleration);
        }

        /// <summary>
        ///   <para>Метод передвижения врага по координатам с помощью синусоиды и интерполяции.</para>
        /// </summary>
        /// <param name="destination">Пункт назначения</param>
        private IEnumerator Jump(Transform destination)
        {
            var startPosition = transform.position;
            var adjustedJumpDuration = jumpDuration / CalculateAcceleration();

            for (float jumpTime = 0; jumpTime < adjustedJumpDuration; jumpTime += Time.deltaTime)
            {
                // Прогресс прыжка от 0 до 1 (как проценты)
                var jumpProgress = jumpTime / adjustedJumpDuration;
                // Высчитывание эффекта параболического прыжка
                var height = Mathf.Sin(Mathf.PI * jumpProgress) * jumpHeight;
                // Перемещение от точки А до Б + эффект параболического прыжка
                transform.position = Vector3.Lerp(startPosition, destination.position, jumpProgress)
                                     + new Vector3(0, height, 0);
                yield return null;
            }

            // Установка следующего места для прыжка
            SetNewDestinationFromQueue();
        }
    }
}