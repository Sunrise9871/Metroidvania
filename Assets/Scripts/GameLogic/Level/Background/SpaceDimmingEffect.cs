using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameLogic.Level.Background
{
    [RequireComponent(typeof(Light2D))]
    public class SpaceDimmingEffect : MonoBehaviour
    {
        [Tooltip("До скольки может уменьшится яркость света")]
        [SerializeField] private float minimalLight;
        
        [Tooltip("Что является землей")]
        [SerializeField] private Transform ground;

        [Tooltip("Задний фон")]
        [SerializeField] private SpriteRenderer background;

        [Tooltip("С какой высоты начнется затемнение")]
        [SerializeField] private float fullOpaqueHeight = 100f;

        [Tooltip("На кокой высоте будет максимальное затемнение")]
        [SerializeField] private float fullTransparentHeight = 450f;

        private Transform _camera;
        private Light2D _light;

        private void Awake()
        {
            if (!UnityEngine.Camera.main)
                throw new NullReferenceException();

            _camera = UnityEngine.Camera.main.transform;
            _light = GetComponent<Light2D>();
        }

        private void Update()
        {
            var straight = 1f - (_camera.position.y - ground.position.y - fullOpaqueHeight) / fullTransparentHeight;
            background.color = new Color(1f, 1f, 1f, straight);

            _light.intensity = Mathf.Clamp(straight, minimalLight, 1f);
        }
    }
}