using UnityEngine;

namespace Shooting.ShootingStyles
{
    public abstract class ShootingStyle
    {
        public abstract ProjectileGeometry GetGeometry(Vector3 direction);
    }
}