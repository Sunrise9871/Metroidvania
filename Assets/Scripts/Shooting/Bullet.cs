using UnityEngine;

namespace Shooting
{
    public class Bullet : MonoBehaviour
    {
        /// <summary>
        /// <para>Устанавливает направление движения projectile и запускает его.</para>
        /// </summary>
        /// <param name="shootDirection">Направление движения/</param>
        public void Setup(Vector3 shootDirection)
        {
            //TODO: убрать константу
            const float moveSpeed = 10f;
            var rb = GetComponent<Rigidbody2D>();
            rb.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);
            //TODO: убрать константу
            Destroy(gameObject, 5f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponent<Target>();
            if (!target) return;
            target.Damage();
            Destroy(gameObject);
        }
    }
}