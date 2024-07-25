using Shooting;
using UnityEngine;

namespace Enemy.EnemyTakingDamageStates
{
    public abstract class EnemyTakingDamageState
    {
        public abstract Color ColorMark { get; }
        
        public abstract bool IsVulnerable(TypeOfFire typeOfFire);
    }
}