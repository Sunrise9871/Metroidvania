using Shooting;

namespace Enemies.EnemyStates
{
    public class EnemyTakingDamagePrimaryState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.PrimaryFire;
    }
}