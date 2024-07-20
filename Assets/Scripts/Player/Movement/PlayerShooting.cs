using System.Collections.Generic;
using Shooting;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private GameObject pfPrimaryBullet;
        [SerializeField] private GameObject pfSecondaryBullet;
        [SerializeField] private GameObject pfCombinedBullet;

        private BulletSpawner _primaryPool;
        private BulletSpawner _secondaryPool;
        private BulletSpawner _combinedPool;

        private Dictionary<TypeOfFire, BulletSpawner> _pools;

        private UnityEngine.Camera _camera;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _camera = UnityEngine.Camera.main;
            
            _primaryPool = new BulletSpawner(pfPrimaryBullet);
            _secondaryPool = new BulletSpawner(pfSecondaryBullet);
            _combinedPool = new BulletSpawner(pfCombinedBullet);

            _pools = new Dictionary<TypeOfFire, BulletSpawner>
            {
                [TypeOfFire.PrimaryFire] = _primaryPool,
                [TypeOfFire.SecondaryFire] = _secondaryPool,
                [TypeOfFire.CombinedFire] = _combinedPool
            };
        }

        private void OnEnable() => _playerInput.Shot += OnShot;

        private void OnDisable() => _playerInput.Shot -= OnShot;

        private void OnShot(TypeOfFire typeOfFire)
        {
            var cursorPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = transform.position.z;
            var direction = cursorPosition - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var pool = _pools[typeOfFire];
            var bulletScript = pool.Get();

            bulletScript.Setup(transform.position, rotation, direction.normalized,
                () => pool.Release(bulletScript));
        }
    }
}