﻿using Shooting;
using Shooting.Bullets;

namespace Enemies.EnemyStates
{
    /// <summary>
    ///   <para>Класс для состояния уязвимости к первичному типу огня</para>
    /// </summary>
    public class EnemyTakingDamagePrimaryState : EnemyTakingDamageState
    {
        public override bool IsVulnerable(TypeOfFire typeOfFire) => typeOfFire == TypeOfFire.PrimaryFire;
    }
}