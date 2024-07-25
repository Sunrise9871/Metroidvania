using Shooting;
using UnityEngine;

namespace Enemy.EnemyTakingDamageStates
{
    public class EnemyTakingDamageSecondaryState : EnemyTakingDamageState
    {
        public override Color ColorMark => Color.blue; 
        
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.SecondaryFire;
    }
}