using Shooting.ShootingStyles;

namespace Enemy.EnemyStages
{
    public class EasyEnemyStage : EnemyStage
    {
        public override ShootingStyle ShootingStyle => new SingleShootingStyle();
        public override float ShootSpeed => 6f;
    }
}