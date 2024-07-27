using UnityEngine;

namespace Enemy.ShootingStyles
{
    public class TripleShootingStyle : ShootingStyle
    {
        private const int Count = 3;
        private const float Yaw = 10f;

        public override ProjectileGeometry GetGeometry(Vector3 direction) => new(direction, Count, Yaw);
    }
}