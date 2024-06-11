using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> platforms;
        [SerializeField] private Transform player;
        [SerializeField] private Transform redZone;
        [SerializeField] private Transform lastPlatformPosition;
        [SerializeField] private float checkLevelRepeatTime;
        
        [Tooltip("Насколько ниже должна быть платформа по сравнению с red zone")]
        [SerializeField] private float redZoneDifference;
        [SerializeField] private int minX, maxX, minY, maxY;

        private Queue<Transform> _activePlatformsQueue;
        private const float MIN_DISTANCE_TO_SPAWN_PLATFORM = 20f;
        
        private void Start()
        {
            _activePlatformsQueue = new Queue<Transform>();
            InvokeRepeating(nameof(GenerateLevel), 0f, checkLevelRepeatTime);
        }

        /// <summary>
        /// Когда генерировать или удалять платформы
        /// </summary>
        private void GenerateLevel()
        {
            //Если red zone выше платформы, то удалить платформу
            while (_activePlatformsQueue.TryPeek(out var highestPlatform))
                if (highestPlatform.position.y < redZone.transform.position.y - redZoneDifference)
                    DestroyLowestPlatform();
                else break;

            //Если игрок приближается к самой высокой платформе, то создать еще платформу
            if (!lastPlatformPosition) return;
            while (Vector3.Distance(player.position, lastPlatformPosition.position) <
                   MIN_DISTANCE_TO_SPAWN_PLATFORM)
                SpawnPlatform();
        }

        /// <summary>
        /// Удаление самой низкой платформы
        /// </summary>
        private void DestroyLowestPlatform() => Destroy(_activePlatformsQueue.Dequeue().gameObject);

        /// <summary>
        /// Создание платформы на новой позиции
        /// </summary>
        private void SpawnPlatform()
        {
            //Вычисление новой позиции для платформы
            var newPosition = new Vector3(
                lastPlatformPosition.position.x + Random.Range(minX, maxX),
                lastPlatformPosition.position.y + Random.Range(minY, maxY),
                lastPlatformPosition.position.z);

            //Выбор и создание случайного префаба платформы
            lastPlatformPosition = Instantiate(platforms[Random.Range(0, platforms.Count)], newPosition,
                Quaternion.identity, gameObject.transform).transform;
            _activePlatformsQueue.Enqueue(lastPlatformPosition);
        }
    }
}