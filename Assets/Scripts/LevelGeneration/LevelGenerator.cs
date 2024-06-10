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

        private Queue<Transform> _activePlatformsQueue;
        private const float MIN_DISTANCE_TO_SPAWN_PLATFORM = 20f;

        private void Start()
        {
            _activePlatformsQueue = new Queue<Transform>();
            InvokeRepeating(nameof(GenerateLevel), 0f, checkLevelRepeatTime);
        }

        private void GenerateLevel()
        {
            if (_activePlatformsQueue.Count > 0)
                while (_activePlatformsQueue.Peek().position.y < redZone.transform.position.y)
                    DestroyLowestPlatform();
            
            //TODO: убрать перегруз
            while (Vector3.Distance(player.position, lastPlatformPosition.position) <
                   MIN_DISTANCE_TO_SPAWN_PLATFORM)
            {
                var newPosition = new Vector3(
                    lastPlatformPosition.position.x + Random.Range(-6, 8),
                    lastPlatformPosition.position.y + Random.Range(3, 7),
                    lastPlatformPosition.position.z);

                lastPlatformPosition = SpawnPlatform(newPosition);
                _activePlatformsQueue.Enqueue(lastPlatformPosition);
            }
        }

        private void DestroyLowestPlatform()
        {
            Destroy(_activePlatformsQueue.Dequeue().gameObject);
        }

        private Transform SpawnPlatform(Vector3 position)
        {
            return Instantiate(platforms[0], position, Quaternion.identity, gameObject.transform).transform;
        }
    }
}