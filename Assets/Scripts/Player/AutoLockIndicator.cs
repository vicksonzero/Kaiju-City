using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace StarterAssets
{
    public class AutoLockIndicator : MonoBehaviour
    {
        public RectTransform _rt;
        public RectTransform sprite;
        public Health target;
        public Camera autoLockCamera;
        public RectTransform canvas;
        public AudioSource sfx;

        public RectTransform summaryRoot;
        public Bars bar;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
        }

        public void SetTarget(Health value, bool allowEffect)
        {
            var isTargetChanging = target != value;
            if (isTargetChanging)
            {
                Play(allowEffect);
                sfx.Play();
                if (target)
                {
                    target.bars = target.bars.Where(x => x != bar).ToArray();
                }

                if (value)
                {
                    value.bars = value.bars.Concat(new[] { bar }).ToArray();
                    value.UpdateBars();
                }
            }

            target = value;
        }

        private void LateUpdate()
        {
            if (!target)
            {
                sprite.localScale = Vector3.zero;
                summaryRoot.gameObject.SetActive(false);
                return;
            }

            summaryRoot.gameObject.SetActive(true);

            var screenPoint = autoLockCamera.WorldToViewportPoint(target.autoAimRoot.position);

            _rt.anchoredPosition = new Vector2(
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