using UnityEngine;

namespace GameLogic.MainLogic
{
    public class GameWinScenario : MonoBehaviour
    {
        [Tooltip("Враг")]
        [SerializeField] private Enemy.Logic.Enemy enemy;

        [Tooltip("UI для проигрыша")]
        [SerializeField] private GameObject winCanvas;

        private void OnEnable() => enemy.Died += OnEnemyDied;

        private void OnDisable() => enemy.Died -= OnEnemyDied;

        private void OnEnemyDied()
        {
            winCanvas.SetActive(true);
        }
    }
}