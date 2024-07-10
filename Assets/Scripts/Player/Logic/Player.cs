using System;
using GameLogic.Interfaces;
using Shooting;
using UnityEngine;

namespace Player.Logic
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float health;

        public event Action PlayerDamaged;
        public event Action PlayerDied;
        
        public void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (health - 1 >= 0)
            {
                health--;
                PlayerDamaged?.Invoke();
            }
            else
                PlayerDied?.Invoke();
        }
    }
}