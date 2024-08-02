using System;
using UnityEngine;

namespace GameLogic.Level.Background
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float parallaxStrength = 0.1f;
        
        private Transform _camera;
        private float _cameraPreviousPosition;

        private void Awake()
        {
            if (!UnityEngine.Camera.main)
                throw new NullReferenceException();

            _camera = UnityEngine.Camera.main.transform;
        }

        private void LateUpdate()
        {
            var delta = _camera.position.y - _cameraPreviousPosition;
            
            _cameraPreviousPosition = _camera.position.y;
            
            var vector3 = transform.position;
            vector3.x = _camera.position.x;
            vector3.y += delta * parallaxStrength; 
            transform.position = vector3;
        }
    }
}