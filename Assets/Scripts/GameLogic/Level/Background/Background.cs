using UnityEngine;

namespace GameLogic.Level.Background
{
    public class Background : MonoBehaviour
    {
        private SpriteRenderer[] _spriteRenderers;
        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            
            ResizeSpriteToScreen();
        }

        private void ResizeSpriteToScreen()
        {
            foreach (var spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.transform.localScale = Vector3.one;
                
                var width = spriteRenderer.sprite.bounds.size.x;
                var height = spriteRenderer.sprite.bounds.size.y;
                
                var worldScreenHeight = _camera.orthographicSize * 2f;
                var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
                
                var xScale = worldScreenWidth / width;
                var yScale = worldScreenHeight / height;
                var newScale = new Vector3(xScale, yScale, 1f);

                spriteRenderer.transform.localScale = newScale;
            }
        }
    }
}