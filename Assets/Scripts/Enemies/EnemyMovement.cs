using System;
using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using UnityEngine;

namespace Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [Tooltip("Первое место назначения")] 
        [SerializeField] private Transform firstDestination;
        [Tooltip("Ноги персонажа (для точных приземлений)")] 
        [SerializeField] private Transform feet;
        [Tooltip("Новое место назначения для прыжка выше текущего не менее, чем на ...")] 
        [SerializeField] private float minHeightForNewDestination = 8f;
        [Tooltip("Сила эффекта параболического прыжка")]
        [SerializeField] private float jumpHeight = 5f;
        [Tooltip("Длительность прыжка")]
        [SerializeField] private float jumpDuration = 0.5f;
        
        private Queue<Transform> _paths; // Очередь путей для врага

        /// <summary>
        ///   <para>Враг получил новое место назначение.</para>
        /// </summary>
        public Action<Transform> OnNewDestinationSet;
        
        private void Awake()
        {
            _paths = new Queue<Transform>();
            LevelGenerator.OnPlatformSpawned += AddNewSpawnedPlatform;
        }

        private void OnDisable() => LevelGenerator.OnPlatformSpawned -= AddNewSpawnedPlatform;

        private void Start() => StartCoroutine(Jump(firstDestination));
        
        /// <summary>
        ///   <para>Добавляет новую созданную на уровне платформу.</para>
        /// </summary>
        /// <param name="spawnedPlatform"></param>
        private void AddNewSpawnedPlatform(Transform spawnedPlatform) => _paths.Enqueue(spawnedPlatform);

        /// <summary>
        ///   <para>Выбирает из очереди и устанавливает новое место назначение для врага.</para>
        /// </summary>
        private void SetNewDestinationFromQueue()
        {
            // Пока есть пути в очереди
            while (_paths.TryDequeue(out var destination))
            {
                // Выбор нового места назначения, который выше не менее чем на minHeightForNewDestination
                if (destination.position.y - transform.position.y < minHeightForNewDestination) continue;
                OnNewDestinationSet(destination);
                StartCoroutine(Jump(destination));
                break;
            }
        }

        /// <summary>
        ///   <para>Метод передвижения врага по координатам с помощью синусоиды и интерполяции.</para>
        /// </summary>
        /// <param name="destination">Пункт назначения</param>
        private IEnumerator Jump(Transform destination)
        {
            var startPosition = transform.position;

            // Время прыжка от 0 до elapsedTime
            for (float jumpTime = 0; jumpTime < jumpDuration; jumpTime += Time.deltaTime)
            {
                // Прогресс прыжка от 0 до 1 (как проценты)
                var jumpProgress = jumpTime / jumpDuration;
                // Высчитывание эффекта параболического прыжка
                var height = Mathf.Sin(Mathf.PI * jumpProgress) * jumpHeight;
                // Перемещение от точки А до Б + эффект параболического прыжка
                transform.position = Vector3.Lerp(startPosition, destination.position, jumpProgress)
                                     + new Vector3(0, height, 0);
                yield return null;
            }
            
            // Ожидание следующего места для прыжка
            SetNewDestinationFromQueue();
        }
    }
}