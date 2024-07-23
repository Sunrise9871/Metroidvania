using UnityEngine;

namespace Enemy.ShotStyles
{
    public class TripleShotStyle : ShotStyle
    {
        private const int Count = 3;
        private const float Yaw = 10f;

        public override ProjectileGeometry GetGeometry(Vector3 direction) => new(direction, Count, Yaw);
    }
}