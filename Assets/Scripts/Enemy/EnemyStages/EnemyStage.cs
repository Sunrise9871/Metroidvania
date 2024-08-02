using Shooting.ShootingStyles;

namespace Enemy.EnemyStages
{
    public abstract class EnemyStage
    {
        public abstract ShootingStyle ShootingStyle { get; }
        public abstract float ShootSpeed { get; }
    }
}