using UnityEngine;

namespace GameLogic.MainLogic
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Player.Logic.Player player;

        private void OnEnable() => player.Died += OnPlayerDied;

        private void OnDisable() => player.Died -= OnPlayerDied;

        private void OnPlayerDied()
        {
            print("Игра окончена!");
        }
    }
}