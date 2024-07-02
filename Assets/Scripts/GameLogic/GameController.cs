using CharacterController;
using UnityEngine;

namespace GameLogic
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Player player;

        private void OnEnable() => player.PlayerDied += OnPlayerDied;

        private void OnDisable() => player.PlayerDied -= OnPlayerDied;

        private void OnPlayerDied()
        {
            print("Игра окончена!");
        }
    }
}