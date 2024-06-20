using Shooting;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Класс для состояния уязвимости к первичному типу огня</para>
    /// </summary>
    public class EnemyPrimaryState : EnemyState
    {
        public override void ReceiveDamage(TypeOfFire typeOfFire)
        {
            if (typeOfFire == TypeOfFire.PrimaryFire)
                UnityEngine.Debug.Log("Урон от " + typeOfFire);
        }
        
    }
}