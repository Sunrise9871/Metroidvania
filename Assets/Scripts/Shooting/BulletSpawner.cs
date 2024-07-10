using UnityEngine;
using UnityEngine.Pool;

namespace Shooting
{
    /// <summary>
    ///   <para>Object pool для объектов со скриптом Bullet</para>
    /// </summary>
    public class BulletSpawner
    {
        private readonly ObjectPool<Bullet> _pool;
        private readonly GameObject _prefab;
        
        private const int DefaultCapacity = 20;
        private const int MaxSize = 100;

        public BulletSpawner(GameObject prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<Bullet>(OnCreateGameObject, OnGetGameObject, OnReleaseGameObject,
                OnDestroyGameObject, false, DefaultCapacity, MaxSize);
        }
        
        /// <summary>
        ///   <para>Получить объект из object pool.</para>
        /// </summary>
        /// <returns>Объект из object pool</returns>
        public Bullet Get() => _pool.Get();
        
        /// <summary>
        /// Вернуть объект в object pool.
        /// </summary>
        /// <param name="obj">Объект для возвращения в object pool</param>
        public void Release(Bullet obj) => _pool.Release(obj);
        
        private Bullet OnCreateGameObject()
        {
            var script = Object.Instantiate(_prefab).GetComponent<Bullet>(); //Скрипт созданного объекта
            script.SetPool(this); //Передача ссылки на object pool созданному объекту
            return script;
        }

        private void OnGetGameObject(Bullet script) => script.gameObject.SetActive(true);
        private void OnReleaseGameObject(Bullet script) => script.gameObject.SetActive(false);
        private void OnDestroyGameObject(Bullet script) => Object.Destroy(script.gameObject);
    }
}