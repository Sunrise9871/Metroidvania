using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> platforms;
        [SerializeField] private Transform player;

        private const float MIN_DISTANCE_TO_SPAWN_PLATFORM = 20f;
        [SerializeField] private Transform lastPlatformPosition;

        private void Update()
        {
            if (Vector3.Distance(player.position, lastPlatformPosition.position) < MIN_DISTANCE_TO_SPAWN_PLATFORM)
            {
                var newPosition = new Vector3(lastPlatformPosition.position.x + Random.Range(-6, 8),
                    lastPlatformPosition.position.y + Random.Range(3, 7),
                    lastPlatformPosition.position.z);
                lastPlatformPosition = SpawnPlatform(newPosition);
            }
        }

        private Transform SpawnPlatform(Vector3 position)
        {
            return Instantiate(platforms[0], position, Quaternion.identity, gameObject.transform).transform;
        }
    }
}