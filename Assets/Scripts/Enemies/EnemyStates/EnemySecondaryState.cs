using Shooting;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Класс для состояния уязвимости к вторичному типу огня</para>
    /// </summary>
    public class EnemySecondaryState : EnemyState
    {
        public override void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (typeOfFire == TypeOfFire.SecondaryFire)
                UnityEngine.Debug.Log("Урон от " + typeOfFire);
        }
    }
}