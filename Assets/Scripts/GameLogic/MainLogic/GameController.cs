using UnityEngine;

namespace GameLogic.MainLogic
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Player.Logic.Player player;

        private void OnEnable() => player.PlayerDied += OnPlayerDied;

        private void OnDisable() => player.PlayerDied -= OnPlayerDied;

        private void OnPlayerDied()
        {
            print("Игра окончена!");
        }
    }
}