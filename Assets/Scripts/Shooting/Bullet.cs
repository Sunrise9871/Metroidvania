using UnityEngine;

namespace Shooting
{
    public class Bullet : MonoBehaviour
    {
        private Vector3 _shootDirection;

        public void Setup(Vector3 shootDirection)
        {
            _shootDirection = shootDirection;
        }
        
        private void Update()
        {
            const float moveSpeed = 10f;
            transform.position += _shootDirection * (moveSpeed * Time.deltaTime);
        }
        
        
    }
}
