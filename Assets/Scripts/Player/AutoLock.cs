using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace StarterAssets
{
    public class AutoLock : MonoBehaviour
    {
        public float scanInterval = 0.5f;
        public Camera autoLockCamera;
        public RectTransform uiCanvas;
        public RectTransform cursor;

        public AutoLockIndicator autoLockIndicator;
        public AutoLockIndicator autoLockCandidateIndicator;

        public Health targetCandidate;
        public Health target;
        public Transform turretBase;

        private PlayerAiming _aiming;

        private StarterAssetsInputs _input;
        private Player _player;
        private bool _lastShootState = false;

        private void Awake()
        {
            _input = FindObjectOfType<StarterAssetsInputs>();
            _aiming = GetComponent<PlayerAiming>();
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            DOVirtual.DelayedCall(scanInterval, UpdateLock, false).SetLoops(-1);
        }

        private void Update()
        {
            if (GamePause.Inst.isPaused) return;
            if (target)
            {
                var screenPoint = autoLockCamera.WorldToViewportPoint(target.autoAimRoot.position);
                if (screenPoint.x is < 0 or > 1 || screenPoint.y is < 0 or > 1)
                {
                    target = null;
                    autoLockIndicator.SetTarget(null, true);
                }
            }

            if (targetCandidate)
            {
                var screenPoint = autoLockCamera.WorldToViewportPoint(targetCandidate.autoAimRoot.position);
                if (screenPoint.x is < 0 or > 1 || screenPoint.y is < 0 or > 1)
                {
                    targetCandidate = null;
                    autoLockCandidateIndicator.SetTarget(null, true);
                }
            }


            var ray = target
                ? new Ray(autoLockCamera.transform.position,
                    target.autoAimRoot.position - autoLockCamera.transform.position)
                : targetCandidate
                    ? new Ray(autoLockCamera.transform.position,
                        targetCandidate.autoAimRoot.position - autoLockCamera.transform.position)
                    : new Ray(turretBase.position,
                        _player.transform.forward);
            _aiming.AimAtRay(ray);

            if (_input.shoot && !GamePause.Inst.isPaused)
            {
                if (!_lastShootState || (!target && targetCandidate))
                {
                    target = targetCandidate;
                    autoLockIndicator.SetTarget(target, true);
                }
            }

            if (!_input.shoot && !GamePause.Inst.isPaused && _lastShootState)
            {
                UpdateLock();
            }

            _lastShootState = _input.shoot;
        }

        public void UpdateLock()
        {
            var center = new Vector2(
                (cursor.anchoredPosition.x) / uiCanvas.sizeDelta.x + 0.5f,
                (cursor.anchoredPosition.y) / uiCanvas.sizeDelta.y + 0.5f
            );
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

            if (autoLockCandidateIndicator != null)
            {
                targetCandidate = closestEnemyResult?.enemy
                    ? closestEnemyResult.enemy.GetComponent<Health>()
                    : null;
                autoLockCandidateIndicator.SetTarget(targetCandidate, false);
            }
        }
    }
}