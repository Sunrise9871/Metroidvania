using System;
using GameLogic.Interfaces;
using Shooting;
using UnityEngine;

namespace Player.Logic
{
    public class Player : MonoBehaviour, IDamageable
    {
        public int Health => health;
        
        [SerializeField] private int health = 12;

        public event Action<int> Damaged;
        public event Action Died;
        
        public void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (health - 1 >= 0)
            {
                health--;
                Damaged?.Invoke(1);
            }
            else
                Died?.Invoke();
        }
    }
}