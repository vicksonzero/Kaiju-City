using System;
using Cinemachine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class KaijuTv : MonoBehaviour
{
    public RectTransform tv;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera bossCamera;
    public CinemachineVirtualCamera bossFramingCamera;
    public CinemachineVirtualCamera giantCamera;

    [CanBeNull]
    private Tween _disableTimer;

    [CanBeNull]
    private Tween _tvSlideTimer;

    private void Awake()
    {
        bossCamera.gameObject.SetActive(false);
        bossFramingCamera.gameObject.SetActive(false);
        giantCamera.gameObject.SetActive(false);
    }

    private void Start()
    {
        tv.anchorMin = new Vector2(tv.anchorMin.x, 1);
    }

    public void ShowBoss(float duration)
    {
        SlideTv(true);
        bossCamera.gameObject.SetActive(true);
        _disableTimer?.Kill();
        _disableTimer = DOVirtual.DelayedCall(duration, ResetTv);
    }

    public void ShowFramedBoss(float duration)
    {
        SlideTv(true);
        bossFramingCamera.gameObject.SetActive(true);
        _disableTimer?.Kill();
        _disableTimer = DOVirtual.DelayedCall(duration, ResetTv);
    }

    public void ShowGiant(float duration)
    {
        SlideTv(true);
        giantCamera.gameObject.SetActive(true);
        _disableTimer?.Kill();
        _disableTimer = DOVirtual.DelayedCall(duration, ResetTv);
    }

    void ResetTv()
    {
        SlideTv(false);
        bossCamera.gameObject.SetActive(false);
        bossFramingCamera.gameObject.SetActive(false);
        giantCamera.gameObject.SetActive(false);
    }

    void SlideTv(bool isShow)
    {
        _tvSlideTimer?.Kill();
        DOVirtual.Float(tv.anchorMin.y, isShow ? 0 : 1, 1, value =>
            tv.anchorMin = new Vector2(tv.anchorMin.x, value));
    }
}