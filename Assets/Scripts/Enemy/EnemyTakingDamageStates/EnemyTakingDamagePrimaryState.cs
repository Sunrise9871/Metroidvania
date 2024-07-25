using Shooting;
using UnityEngine;

namespace Enemy.EnemyTakingDamageStates
{
    public class EnemyTakingDamagePrimaryState : EnemyTakingDamageState
    {
        public override Color ColorMark => Color.red;
        
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.PrimaryFire;
    }
}