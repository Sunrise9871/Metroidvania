using UnityEngine;

namespace Enemy.ShotStyles
{
    public class SingleShotStyle : ShotStyle
    {
        private const int Count = 1;
        private const float Yaw = 0f;

        public override ProjectileGeometry GetGeometry(Vector3 direction) => new(direction, Count, Yaw);
    }
}