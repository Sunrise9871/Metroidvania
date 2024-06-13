using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [Tooltip("Список платформ для генерации уровня")]
        [SerializeField] private List<GameObject> platforms;
        [SerializeField] private Transform player;
        [SerializeField] private Transform redZone;
        [SerializeField] private Transform lastPlatform;
        [SerializeField] private float checkLevelRepeatTime;
        
        [Tooltip("Насколько ниже должна быть платформа по сравнению с red zone")]
        [SerializeField] private float redZoneDifference;
        [SerializeField] private int minX, maxX, minY, maxY;

        private Queue<Transform> _activePlatformsQueue;
        private const float MIN_DISTANCE_TO_SPAWN_PLATFORM = 20f;
        
        public static Action<Transform> OnPlatformSpawned;
        
        private void Awake()
        {
            _activePlatformsQueue = new Queue<Transform>();
            InvokeRepeating(nameof(GenerateLevel), 0f, checkLevelRepeatTime);
        }

        /// <summary>
        ///   <para>Когда генерировать или удалять платформы</para>
        /// </summary>
        private void GenerateLevel()
        {
            //Если red zone выше платформы, то удалить платформу
            while (_activePlatformsQueue.TryPeek(out var lowestPlatform))
                if (lowestPlatform.position.y < redZone.transform.position.y - redZoneDifference)
                    DestroyLowestPlatform();
                else break;

            //Если игрок приближается к самой высокой платформе, то создать еще платформу
            if (!lastPlatform) return;
            while (lastPlatform.position.y < player.position.y + MIN_DISTANCE_TO_SPAWN_PLATFORM)
                SpawnPlatform();
        }

        /// <summary>
        ///   <para>Удаление самой низкой платформы</para>
        /// </summary>
        private void DestroyLowestPlatform() => Destroy(_activePlatformsQueue.Dequeue().gameObject);

        /// <summary>
        ///   <para>Создание платформы на новой позиции</para>
        /// </summary>
        private void SpawnPlatform()
        {
            //Вычисление новой позиции для платформы
            var newPosition = new Vector3(
                Random.Range(minX, maxX),
                lastPlatform.position.y + Random.Range(minY, maxY),
                lastPlatform.position.z);

            //Выбор и создание случайного префаба платформы
            lastPlatform = Instantiate(platforms[Random.Range(0, platforms.Count)], newPosition,
                Quaternion.identity, gameObject.transform).transform;
            _activePlatformsQueue.Enqueue(lastPlatform);
            
            OnPlatformSpawned?.Invoke(lastPlatform);
        }
    }
}