using UnityEngine;

namespace Shooting.ShootingStyles
{
    public class CircleShootingStyle : ShootingStyle
    {
        private const int Count = 36;
        private const float Yaw = 10f;

        public override ProjectileGeometry GetGeometry(Vector3 direction) => new(direction, Count, Yaw);
    }
}