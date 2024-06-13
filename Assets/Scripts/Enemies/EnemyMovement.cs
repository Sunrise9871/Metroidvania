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
        [Tooltip("Сила эффекта параболического прыжка")]
        [SerializeField] private float jumpHeight = 5f;
        [Tooltip("Длительность прыжка")]
        [SerializeField] private float jumpDuration = 0.5f;
        
        private Queue<Transform> _paths; // Очередь путей для врага
        
        private void Awake()
        {
            _paths = new Queue<Transform>();
            LevelGenerator.OnPlatformSpawned += AddNewSpawnedPlatform;
        }

        private void OnDisable() => LevelGenerator.OnPlatformSpawned -= AddNewSpawnedPlatform;

        private void Start()
        {
            StartCoroutine(Jump(firstDestination));
        }

        /// <summary>
        /// Добавляет новую созданную на уровне платформу
        /// </summary>
        /// <param name="spawnedPlatform"></param>
        private void AddNewSpawnedPlatform(Transform spawnedPlatform) => _paths.Enqueue(spawnedPlatform);

        /// <summary>
        /// Устанавливает новое место назначение для врага
        /// </summary>
        private void SetNewDestinationFromQueue()
        {
            while (_paths.TryDequeue(out var platform))
            {
                if (platform.position.y - gameObject.transform.position.y < 8) continue;
                StartCoroutine(Jump(platform));
                break;
            }
        }

        /// <summary>
        /// Метод передвижения врага по координатам с помощью синусоиды и интерполяции.
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