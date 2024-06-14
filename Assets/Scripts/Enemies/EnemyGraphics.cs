using UnityEngine;

namespace Enemies
{
    public class EnemyGraphics : MonoBehaviour
    {
        private EnemyMovement _enemyMovement;

        private readonly Vector3 _leftSideSprite = new(-1f, 1f, 1f);
        private readonly Vector3 _rightSideSprite = new(1f, 1f, 1f);

        private void Awake() => _enemyMovement = GetComponent<EnemyMovement>();
        private void OnEnable() => _enemyMovement.OnNewDestinationSet += SetSpriteSide;
        private void OnDisable() => _enemyMovement.OnNewDestinationSet -= SetSpriteSide;

        private void SetSpriteSide(Transform destination)
        {
            transform.localScale = destination.position.x < transform.position.x
                ? _leftSideSprite
                : _rightSideSprite;
        }
    }
}