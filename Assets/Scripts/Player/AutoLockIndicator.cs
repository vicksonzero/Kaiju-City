using System;
using DG.Tweening;
using UnityEngine;

namespace StarterAssets
{
    public class AutoLockIndicator : MonoBehaviour
    {
        public RectTransform sprite;
        public Health target;
        public Camera autoLockCamera;
        public RectTransform canvas;
        public AudioSource sfx;
        
        
        public void SetTarget(Health value, bool allowEffect)
        {
            if (target != value)
            {
                Play(allowEffect);
                sfx.Play();
            }

            target = value;
        }

        private void LateUpdate()
        {
            if (!target)
            {
                sprite.localScale = Vector3.zero;
                return;
            }

            var screenPoint = autoLockCamera.WorldToViewportPoint(target.autoAimRoot.position);

            sprite.anchoredPosition = new Vector2(
                (screenPoint.x - 0.5f) * canvas.sizeDelta.x,
                (screenPoint.y - 0.5f) * canvas.sizeDelta.y
            );
        }

        public void Play(bool allowEffect)
        {
            if (!allowEffect)
            {
                sprite.localScale = Vector3.one;
            }
            else
            {
                sprite.localScale = Vector3.one * 2f;
                sprite.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCubic);
            }
        }
    }
}