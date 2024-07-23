using UnityEngine;

namespace Enemy.ShotStyles
{
    public abstract class ShotStyle
    {
        public abstract ProjectileGeometry GetGeometry(Vector3 direction);
    }
}