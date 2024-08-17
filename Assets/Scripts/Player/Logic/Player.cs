using System;
using GameLogic.Interfaces;
using GameLogic.MainLogic;
using Shooting;
using UnityEngine;

namespace Player.Logic
{
    public class Player : MonoBehaviour, IDamageable
    {
        [Tooltip("Количество очков здоровья у игрока. 4 очка здоровья = 1 сердце")]
        [SerializeField] private int health = 12;

        private GameStopScenario _stopScenario;

        public int Health => health;

        public event Action<int> Damaged;
        public event Action Died;
        
        private void Awake() => _stopScenario = FindAnyObjectByType<GameStopScenario>();

        private void OnEnable() => _stopScenario.GameStopped += OnGameStopped;

        private void OnDisable() => _stopScenario.GameStopped -= OnGameStopped;
        
        public void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (health - 1 > 0)
            {
                health--;
                Damaged?.Invoke(1);
            }
            else
                Died?.Invoke();
        }

        public void Die() => Died?.Invoke();

        private void OnGameStopped() => enabled = false;
    }
}