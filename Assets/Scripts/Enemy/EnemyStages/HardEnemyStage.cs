using Shooting.ShootingStyles;

namespace Enemy.EnemyStages
{
    public class HardEnemyStage : EnemyStage
    {
        public override ShootingStyle ShootingStyle => new QuintupleShootingStyle();
        public override float ShootSpeed => 2f;
    }
}