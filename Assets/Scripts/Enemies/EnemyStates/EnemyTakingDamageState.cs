using Shooting.Bullets;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Абстрактный класс для определения уязвимости к типам огня у врага.</para>
    /// </summary>
    public abstract class EnemyTakingDamageState
    {
        /// <summary>
        ///   <para>Определяет, является ли объект уязвимым к данному типу атаки.</para>
        /// </summary>
        /// <param name="typeOfFire">Тип атаки</param>
        public abstract bool IsVulnerable(TypeOfFire typeOfFire);
    }
}