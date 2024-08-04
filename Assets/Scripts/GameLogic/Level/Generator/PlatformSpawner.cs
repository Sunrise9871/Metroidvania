using UnityEngine;
using UnityEngine.Pool;

namespace GameLogic.Level.Generator
{
    public class PlatformSpawner
    {
        private const int DefaultCapacity = 20;
        private const int MaxSize = 100;
        
        private readonly ObjectPool<Transform> _pool;
        private readonly Transform _prefab;

        public PlatformSpawner(Transform prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<Transform>(OnCreateGameObject, OnGetGameObject, OnReleaseGameObject,
                OnDestroyGameObject, false, DefaultCapacity, MaxSize);
        }
        
        public Transform Get() => _pool.Get();
        
        public void Release(Transform transform) => _pool.Release(transform);

        private Transform OnCreateGameObject() => Object.Instantiate(_prefab);

        private void OnGetGameObject(Transform transform) => transform.gameObject.SetActive(true);
        private void OnReleaseGameObject(Transform transform) => transform.gameObject.SetActive(false);
        private void OnDestroyGameObject(Transform transform) => Object.Destroy(transform);
    }
}