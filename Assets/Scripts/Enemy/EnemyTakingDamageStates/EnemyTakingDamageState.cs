using Shooting;

namespace Enemy.EnemyTakingDamageStates
{
    public abstract class EnemyTakingDamageState
    {
        public abstract bool IsVulnerable(TypeOfFire typeOfFire);
    }
}