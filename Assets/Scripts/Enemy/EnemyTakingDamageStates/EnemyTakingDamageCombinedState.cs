using Shooting;

namespace Enemy.EnemyTakingDamageStates
{
    public class EnemyTakingDamageCombinedState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.CombinedFire;
    }
}