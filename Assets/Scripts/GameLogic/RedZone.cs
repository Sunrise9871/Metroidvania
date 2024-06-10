using UnityEngine;

namespace GameLogic
{
    public class RedZone : MonoBehaviour
    {
        [SerializeField] private float speed;

        private void Update()
        {
            gameObject.transform.Translate(new Vector3(0f, speed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            print("Проигрыш!");
        }
    }
}