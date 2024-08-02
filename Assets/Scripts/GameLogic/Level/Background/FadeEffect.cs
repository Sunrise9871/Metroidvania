using System;
using UnityEngine;

namespace GameLogic.Level.Background
{
    public class FadeEffect : MonoBehaviour
    {
        [SerializeField] private Transform ground;
        [SerializeField] private float fullOpaqueHeight = 100f;
        [SerializeField] private float fullTransparentHeight = 450f;    

        private Transform _camera;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            if (!UnityEngine.Camera.main)
                throw new NullReferenceException();

            _camera = UnityEngine.Camera.main.transform;
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var alpha = 1f - (_camera.position.y - ground.position.y - fullOpaqueHeight) / fullTransparentHeight;
            _sprite.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}