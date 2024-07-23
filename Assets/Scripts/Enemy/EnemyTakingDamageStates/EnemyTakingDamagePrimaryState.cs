using Shooting;

namespace Enemy.EnemyTakingDamageStates
{
    public class EnemyTakingDamagePrimaryState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.PrimaryFire;
    }
}