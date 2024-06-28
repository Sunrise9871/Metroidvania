using Shooting;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Класс для состояния уязвимости к вторичному типу огня</para>
    /// </summary>
    public class EnemyTakingDamageSecondaryState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.SecondaryFire;
    }
}