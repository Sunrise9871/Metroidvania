using UnityEngine;
using UnityEngine.Pool;

namespace CustomUnityPool
{
    public class CustomUnityPool
    {
        private ObjectPool<GameObject> _pool;
        private GameObject _prefab;

        public CustomUnityPool(GameObject prefab, int preWarmObjectCount)
        {
            _prefab = prefab;
            _pool = new ObjectPool<GameObject>(OnCreateGameObject, OnGetGameObject, OnReleaseGameObject,
                OnDestroyGameObject, false, preWarmObjectCount);
        }

        public GameObject Get()
        {
            return _pool.Get();
        }

        public void Release(GameObject obj)
        {
            _pool.Release(obj);
        }
        
        private GameObject OnCreateGameObject()
        {
            return Object.Instantiate(_prefab);
        }

        private void OnGetGameObject(GameObject obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseGameObject(GameObject obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyGameObject(GameObject obj)
        { 
            Object.Destroy(obj);
        }
    }
}