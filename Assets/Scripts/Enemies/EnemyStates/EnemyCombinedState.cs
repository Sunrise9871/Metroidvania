using Shooting;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Класс для состояния уязвимости к комбинированному типу огня</para>
    /// </summary>
    public class EnemyCombinedState : EnemyState
    {
        public override void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (typeOfFire == TypeOfFire.CombinedFire)
                UnityEngine.Debug.Log("Урон от " + typeOfFire);
        }
    }
}