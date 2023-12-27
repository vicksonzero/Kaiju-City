using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace StarterAssets
{
    public class AutoLockBuilding : MonoBehaviour
    {
        public float scanInterval = 0.5f;
        public Camera autoLockCamera;
        public RectTransform uiCanvas;
        public RectTransform cursor;

        public AutoLockIndicator autoLockBuildingIndicator;

        public Health targetCandidate;
        public Transform turretBase;
        public LayerMask buildingLayer;

        private StarterAssetsInputs _input;
        private Player _player;

        private void Awake()
        {
            _input = FindObjectOfType<StarterAssetsInputs>();
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            DOVirtual.DelayedCall(scanInterval, UpdateLock, false).SetLoops(-1);
        }

        private void Update()
        {
            if (targetCandidate)
            {
                var screenPoint = autoLockCamera.WorldToViewportPoint(targetCandidate.autoAimRoot.position);
                if (screenPoint.x is < 0 or > 1 || screenPoint.y is < 0 or > 1)
                {
                    targetCandidate = null;
                    autoLockBuildingIndicator.SetTarget(null, true);
                }
            }
        }

        public void UpdateLock()
        {
            if (!autoLockBuildingIndicator) return;
            var center = new Vector2(
                (cursor.anchoredPosition.x) / uiCanvas.sizeDelta.x + 0.5f,
                (cursor.anchoredPosition.y) / uiCanvas.sizeDelta.y + 0.5f
            );
            var hasHit = Physics.Raycast(
                autoLockCamera.ViewportPointToRay(center),
                out var hitInfo, 30, buildingLayer);

            if (hasHit)
            {
                var building = hitInfo.collider.GetComponentInParent<Building>();
                var health = building.GetComponentInParent<Health>();
                if (health)
                {
                    targetCandidate = health;
                    autoLockBuildingIndicator.SetTarget(targetCandidate, false);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!cursor) return;
            var center = new Vector2(
                (cursor.anchoredPosition.x) / uiCanvas.sizeDelta.x + 0.5f,
                (cursor.anchoredPosition.y) / uiCanvas.sizeDelta.y + 0.5f
            );
            var ray = autoLockCamera.ViewportPointToRay(center);

            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 3);
        }
    }
}