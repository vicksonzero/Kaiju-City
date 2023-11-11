using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace StarterAssets
{
    public class AutoLock : MonoBehaviour
    {
        public Camera autoLockCamera;
        public RectTransform ui;

        public AutoLockIndicator autoLockIndicator;

        public Transform target;

        private PlayerAiming _aiming;

        private void Start()
        {
            _aiming = GetComponent<PlayerAiming>();
            DOVirtual.DelayedCall(1, UpdateLock).SetLoops(-1);
        }

        private void Update()
        {
            if (target)
            {
                var ray = new Ray(autoLockCamera.transform.position,
                    target.position - autoLockCamera.transform.position);
                _aiming.AimAtRay(ray);
            }
        }

        public void UpdateLock()
        {
            var center = new Vector2(0.5f, 0.5f);
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            var closestEnemyResult = enemies
                .Select(x => new
                {
                    enemy = x,
                    screenPoint = autoLockCamera.WorldToViewportPoint(x.transform.position),
                })
                .Where(x => x.screenPoint.z is > 0 and < 30f)
                .Where(x => x.screenPoint.x is > 0 and < 1)
                .Where(x => x.screenPoint.y is > 0 and < 1)
                // .Select(x => new
                // {
                //     x.enemy,
                //     screenPoint = new Vector3(x.screenPoint.x, x.screenPoint.y / 5f, x.screenPoint.z),
                // })
                // .Where(x => Physics.Linecast(
                //     autoLockCamera.transform.position,
                //     x.enemy.transform.position, out var hitInfo))
                .OrderBy(x => Vector2.Distance(x.screenPoint, center))
                .FirstOrDefault();

            if (autoLockIndicator != null)
            {
                var closestEnemy = closestEnemyResult?.enemy ? closestEnemyResult.enemy : null;
                target = closestEnemy?.transform;
                autoLockIndicator.SetTarget(target);
            }
        }
    }
}