using Shooting;

namespace Enemies.EnemyStates
{
    public class EnemyTakingDamageCombinedState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.CombinedFire;
    }
}