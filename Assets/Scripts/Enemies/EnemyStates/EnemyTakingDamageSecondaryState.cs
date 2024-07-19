using Shooting;

namespace Enemies.EnemyStates
{
    public class EnemyTakingDamageSecondaryState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.SecondaryFire;
    }
}