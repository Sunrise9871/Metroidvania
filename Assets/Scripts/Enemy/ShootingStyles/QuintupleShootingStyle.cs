using UnityEngine;

namespace Enemy.ShootingStyles
{
    public class QuintupleShootingStyle : ShootingStyle
    {
        private const int Count = 5;
        private const float Yaw = 20f;

        public override ProjectileGeometry GetGeometry(Vector3 direction) => new(direction, Count, Yaw);
    }
}