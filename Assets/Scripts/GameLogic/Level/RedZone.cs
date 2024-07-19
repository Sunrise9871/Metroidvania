using UnityEngine;

namespace GameLogic.Level
{
    public class RedZone : MonoBehaviour
    {
        [Tooltip("Скорость, с которой красная зона поднимается по уровню")]
        [SerializeField] private float speed;

        private void Update() => gameObject.transform.Translate(Vector3.up * (speed * Time.deltaTime));

        //Layer: Player
        private void OnTriggerEnter2D(Collider2D other)
        {
            print("Проигрыш!");
        }
    }
}