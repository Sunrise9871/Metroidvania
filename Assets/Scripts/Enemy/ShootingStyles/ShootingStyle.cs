using UnityEngine;

namespace Enemy.ShootingStyles
{
    public abstract class ShootingStyle
    {
        public abstract ProjectileGeometry GetGeometry(Vector3 direction);
    }
}