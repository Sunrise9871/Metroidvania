using UnityEngine;

namespace Shooting
{
    public class Bullet : MonoBehaviour
    {
        [Tooltip("Скорость полета projectile.")]
        [SerializeField] private float moveSpeed = 10f;
        [Tooltip("Время до уничтожения projectile в секундах.")]
        [SerializeField] private float timeToDestroy = 5f;
        
        /// <summary>
        /// <para>Устанавливает направление движения projectile и запускает его.</para>
        /// </summary>
        /// <param name="shootDirection">Направление движения/</param>
        public void Setup(Vector3 shootDirection)
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);
            Destroy(gameObject, timeToDestroy);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Уничтожения, когда входит в триггер (врага)
            var target = other.GetComponent<Target>();
            if (!target) return;
            target.Damage();
            Destroy(gameObject);
        }
    }
}