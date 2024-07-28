using Enemy.ShootingStyles;

namespace Enemy.EnemyStages
{
    public class RageEnemyStage : EnemyStage
    {
        public override ShootingStyle ShootingStyle => new CircleShootingStyle();
        public override float ShootSpeed => 2f;
    }
}