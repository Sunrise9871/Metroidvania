using Shooting;

namespace Enemies.EnemyStates
{
    public abstract class EnemyTakingDamageState
    {
        public abstract bool IsVulnerable(TypeOfFire typeOfFire);
    }
}