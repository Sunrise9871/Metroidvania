using Shooting;
using UnityEngine;

namespace Enemy.EnemyTakingDamageStates
{
    public class EnemyTakingDamageCombinedState : EnemyTakingDamageState
    {
        public override Color ColorMark => Color.white;
        
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.CombinedFire;
    }
}