using System;
using DG.Tweening;
using UnityEngine;

namespace StarterAssets
{
    public class AutoLockIndicator : MonoBehaviour
    {
        public RectTransform sprite;
        public Transform target;
        public Camera autoLockCamera;
        public RectTransform canvas;

        public void SetTarget(Transform value)
        {
            if (target != value)
                Play();

            target = value;
        }

        private void LateUpdate()
        {
            if (!target)
            {
                sprite.localScale = Vector3.zero;
                return;
            }

            var screenPoint = autoLockCamera.WorldToViewportPoint(target.position);

            sprite.anchoredPosition = new Vector2(
                (screenPoint.x - 0.5f) * canvas.sizeDelta.x,
                (screenPoint.y - 0.5f) * canvas.sizeDelta.y
            );
        }

        public void Play()
        {
            sprite.localScale = Vector3.one * 2f;
            sprite.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCubic);
        }
    }
}