using Shooting;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Абстрактный класс для определения уязвимости к типам огня у врага.</para>
    /// </summary>
    public abstract class EnemyState
    {
        /// <summary>
        ///   <para>Получает информацию о столкнувшемся projectile игрока.</para>
        /// </summary>
        /// <param name="typeOfFire"></param>
        public abstract void ReceiveDamage(TypeOfFire typeOfFire);
    }
}