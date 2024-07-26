using UnityEngine;

namespace GameLogic.MainLogic
{
    public class GameLossScenario : MonoBehaviour
    {
        [Tooltip("HUD игры")]
        [SerializeField] private GameObject hud;
        
        [Tooltip("Игрок")]
        [SerializeField] private Player.Logic.Player player;

        [Tooltip("UI для проигрыша")]
        [SerializeField] private GameObject lossCanvas;

        private void OnEnable() => player.Died += OnPlayerDied;

        private void OnDisable() => player.Died -= OnPlayerDied;

        private void OnPlayerDied()
        {
            hud.SetActive(false);
            lossCanvas.SetActive(true);
        }
    }
}