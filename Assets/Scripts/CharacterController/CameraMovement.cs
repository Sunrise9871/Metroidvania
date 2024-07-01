using UnityEngine;

namespace CharacterController
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime = 0.25f;

        private readonly Vector3 _offset = new Vector3(0f, 0f, -10f);
        private Vector3 _velocity = Vector3.zero;

        private void Update()
        {
            var targetPosition = target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        }
    }
}