using System;
using System.Collections;
using UnityEngine;

namespace GameLogic.MainLogic
{
    public class GameStopScenario : MonoBehaviour
    {
        private const float StopGameTime = 3f;
        
        private readonly WaitForSeconds _wait = new(StopGameTime);

        [Tooltip("HUD игры")]
        [SerializeField] private GameObject hud;

        [Tooltip("Игрок")]
        [SerializeField] private Player.Logic.Player player;

        [Tooltip("Враг")]
        [SerializeField] private Enemy.Logic.Enemy enemy;

        [Tooltip("UI для выигрыша")]
        [SerializeField] private GameObject winCanvas;

        [Tooltip("UI для проигрыша")]
        [SerializeField] private GameObject lossCanvas;
        
        public event Action GameStopped;

        private void Awake() => Time.timeScale = 1f;
        
        private void OnEnable()
        {
            player.Died += OnPlayerDied;
            enemy.Died += OnEnemyDied;
        }

        private void OnDisable()
        {
            player.Died -= OnPlayerDied;
            enemy.Died -= OnEnemyDied;
        }

        private void OnEnemyDied() => StartCoroutine(EndGame(winCanvas));

        private void OnPlayerDied() => StartCoroutine(EndGame(lossCanvas));

        private IEnumerator EndGame(GameObject canvas)
        {
            GameStopped?.Invoke();

            yield return _wait;

            hud.SetActive(false);
            canvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}