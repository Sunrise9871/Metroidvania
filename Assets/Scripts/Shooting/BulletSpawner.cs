using UnityEngine;
using UnityEngine.Pool;

namespace Shooting
{
    public class BulletSpawner
    {
        private const int DefaultCapacity = 20;
        private const int MaxSize = 100;
        
        private readonly ObjectPool<Bullet> _pool;
        private readonly GameObject _prefab;

        public BulletSpawner(GameObject prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<Bullet>(OnCreateGameObject, OnGetGameObject, OnReleaseGameObject,
                OnDestroyGameObject, false, DefaultCapacity, MaxSize);
        }
        
        public Bullet Get() => _pool.Get();
        
        public void Release(Bullet obj) => _pool.Release(obj);
        
        private Bullet OnCreateGameObject() => Object.Instantiate(_prefab).GetComponent<Bullet>();

        private void OnGetGameObject(Bullet script) => script.gameObject.SetActive(true);
        private void OnReleaseGameObject(Bullet script) => script.gameObject.SetActive(false);
        private void OnDestroyGameObject(Bullet script) => Object.Destroy(script.gameObject);
    }
}