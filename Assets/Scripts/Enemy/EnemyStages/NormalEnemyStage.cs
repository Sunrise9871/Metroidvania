using Shooting.ShootingStyles;

namespace Enemy.EnemyStages
{
    public class NormalEnemyStage : EnemyStage
    {
        public override ShootingStyle ShootingStyle => new TripleShootingStyle();
        public override float ShootSpeed => 4f;
    }
}