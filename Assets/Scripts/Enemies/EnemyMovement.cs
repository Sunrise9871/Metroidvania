using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using UnityEngine;

namespace Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private Transform firstDestination;
        [SerializeField] private Transform feet;

        private Queue<Transform> _paths;
        private bool _isJumping;

        private void Awake()
        {
            _paths = new Queue<Transform>();
            LevelGenerator.OnPlatformSpawned += AddNewSpawnedPlatform;
        } 
        private void OnDisable() => LevelGenerator.OnPlatformSpawned -= AddNewSpawnedPlatform;

        private void Start() => StartCoroutine(Jump(firstDestination));
        private void AddNewSpawnedPlatform(Transform spawnedPlatform)
        {
            _paths.Enqueue(spawnedPlatform);
        }

        private void SetNewDestination()
        {
            while (_paths.TryDequeue(out var platform))
            {
                print(gameObject.transform.position.y - platform.position.y);
                if (platform.position.y - gameObject.transform.position.y < 8) continue;
                StartCoroutine(Jump(platform));
                break;
            }
        }

        private IEnumerator Jump(Transform currentPath)
        {
            while (Vector3.Distance(feet.position, currentPath.position) > 0.1)
            {
                print(Vector3.Distance(feet.position, currentPath.position));
                var direction = currentPath.position - feet.position;
                gameObject.transform.Translate(direction.normalized * (speed * Time.deltaTime));
                yield return null;
            }

            SetNewDestination();
        }
    }
}