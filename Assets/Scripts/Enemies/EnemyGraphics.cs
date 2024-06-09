using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyGraphics : MonoBehaviour
    {
        [SerializeField] private AIPath aiPath;

        private void Start()
        {
            aiPath = GetComponent<AIPath>();
        }

        private void Update()
        {
            transform.localScale = aiPath.desiredVelocity.x switch
            {
                >= 0.01f => new Vector3(-1f, 1f, 1f),
                <= 0.01f => new Vector3(1f, 1f, 1f),
                _ => transform.localScale
            };
        }
    }
}
