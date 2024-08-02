using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Shooting.ShootingStyles
{
    public class ProjectileGeometry
    {
        public readonly ReadOnlyCollection<Vector3> Directions;
        public readonly ReadOnlyCollection<Quaternion> Rotations;

        public ProjectileGeometry(Vector3 playerDirection, int count, float yaw)
        {
            var directionsList = CalculateDirections(playerDirection, count, yaw);
            Directions = directionsList.AsReadOnly();
            Rotations = CalculateRotations(directionsList, yaw).AsReadOnly();
        }

        private List<Vector3> CalculateDirections(Vector3 playerDirection, int count, float yaw)
        {
            switch (count)
            {
                case <= 0:
                    throw new InvalidOperationException();
                case 1:
                    return new List<Vector3> { playerDirection };
            }

            var halfCount = count / 2;
            float initialAngle;
            if (count % 2 != 0)
                initialAngle = halfCount * -yaw;
            else
                initialAngle = (halfCount - 1) * -yaw - yaw / 2f;

            var directions = new List<Vector3>();
            for (var i = 0; i < count; i++)
            {
                var direction = Quaternion.Euler(0, 0, initialAngle + i * yaw) * playerDirection;
                directions.Add(direction);
            }

            return directions;
        }

        private List<Quaternion> CalculateRotations(List<Vector3> playerDirections, float yaw)
        {
            var rotations = new List<Quaternion>();
            
            var initialAngle = Mathf.Atan2(playerDirections[0].y, playerDirections[0].x) * Mathf.Rad2Deg;
            var initialRotation = Quaternion.Euler(new Vector3(0, 0, initialAngle));
            rotations.Add(initialRotation);

            for (var i = 1; i < playerDirections.Count; i++)
            {
                var angle = Quaternion.Euler(new Vector3(0, 0, initialAngle + i * yaw));
                rotations.Add(angle);
            }

            return rotations;
        }
    }
}