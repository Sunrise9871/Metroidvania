using Shooting;
using Shooting.Bullets;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Класс для состояния уязвимости к комбинированному типу огня</para>
    /// </summary>
    public class EnemyTakingDamageCombinedState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.CombinedFire;
    }
}