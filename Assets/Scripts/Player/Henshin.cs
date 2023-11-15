using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Henshin : MonoBehaviour
{
    public HenshinState henshinState;
    public Transform cameraFollowRoot;
    public RectTransform henshinButton;

    [Header("Tank")]
    public Transform tankTransform;

    public ThirdPersonTankController tankInput;

    public CinemachineVirtualCamera tankCamera;
    public Transform tankCameraFollowRoot;

    [Header("Giant")]
    public Transform giantTransform;

    public ThirdPersonController giantInput;

    public CinemachineVirtualCamera giantCamera;
    public Transform giantCameraFollowRoot;

    private StarterAssetsInputs _input;


    public enum HenshinState
    {
        Tank,
        Giant,
    }

    private void Start()
    {
        _input = FindObjectOfType<StarterAssetsInputs>();
        ToggleHenshin(HenshinState.Tank);
        henshinButton.gameObject.SetActive(false);
        DOVirtual.DelayedCall(60, () => henshinButton.gameObject.SetActive(true));
    }

    private void Update()
    {
        if (_input.henshinDown)
        {
            ToggleHenshin();
        }
    }

    public void ToggleHenshin() => ToggleHenshin(henshinState == HenshinState.Tank
        ? HenshinState.Giant
        : HenshinState.Tank);

    public void ToggleHenshin(HenshinState newState)
    {
        cameraFollowRoot.SetParent(newState == HenshinState.Tank
                ? tankCameraFollowRoot
                : giantCameraFollowRoot,
            false);

        tankInput.enabled = newState == HenshinState.Tank;
        giantInput.enabled = newState == HenshinState.Giant;

        tankTransform.gameObject.SetActive(newState == HenshinState.Tank);
        giantTransform.gameObject.SetActive(newState == HenshinState.Giant);

        tankCamera.gameObject.SetActive(newState == HenshinState.Tank);
        giantCamera.gameObject.SetActive(newState == HenshinState.Giant);

        if (newState == HenshinState.Tank && henshinState != HenshinState.Tank)
        {
            tankTransform.position = giantCameraFollowRoot.position;
            tankTransform.rotation = giantTransform.rotation;
            tankCamera.transform.position = giantCamera.transform.position;
        }
        else
        {
            giantInput.enabled = false;
            giantTransform.position = tankTransform.position;
            giantTransform.rotation = tankTransform.rotation;
            giantCamera.transform.position = tankCamera.transform.position;
            giantTransform.DOScale(Vector3.one, 3f)
                .From(Vector3.one * 0.1f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => { giantInput.enabled = true; });
        }

        henshinState = newState;
    }
}