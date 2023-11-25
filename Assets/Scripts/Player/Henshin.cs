using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using EditorCools;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Henshin : MonoBehaviour
{
    public float energy = 0;
    public float energyMax = 60;
    public bool henshinTimerDone = false;

    public Bars enBar;
    public Bars enTimerBar;
    public Bars[] bars;

    public delegate void OnHenshinChanged(bool isGiant);

    public OnHenshinChanged HenshinChanged;

    public HenshinState henshinState;
    public Transform cameraFollowRoot;
    public RectTransform henshinButton;
    public RectTransform henshinPcLabel;

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
        henshinPcLabel.gameObject.SetActive(false);

        enBar.gameObject.SetActive(true);
        enTimerBar.gameObject.SetActive(false);
        DOVirtual.DelayedCall(90, () =>
        {
            henshinTimerDone = true;
            TryShowHenshin();
        });


        foreach (var bar in bars)
        {
            bar.Init(energy, energyMax);
        }
    }

    private void Update()
    {
        if (_input.henshinDown && CanHenshin())
        {
            ToggleHenshin();
        }

        if (henshinState == HenshinState.Giant)
        {
            AddEnergy(-Time.deltaTime);
        }
    }

    [Button()]
    public void ForceShowHenshin()
    {
        AddEnergy(60);
        henshinTimerDone = true;
        TryShowHenshin();
    }

    public void AddEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, energyMax);

        foreach (var bar in bars)
        {
            bar.SetValue(energy, energyMax);
        }


        if (energy <= 0)
        {
            TryDeactivateHenshin();
        }
        else
        {
            TryShowHenshin();
        }
    }

    bool CanHenshin()
    {
        if (!henshinTimerDone) return false;
        if (henshinState == HenshinState.Giant) return false;
        return true;
    }

    public bool TryShowHenshin()
    {
        if (!henshinTimerDone) return false;
        if (henshinState == HenshinState.Giant) return false;

#if (UNITY_IOS || UNITY_ANDROID)
            henshinButton.gameObject.SetActive(true);
#else
        henshinPcLabel.gameObject.SetActive(true);
#endif
        return true;
    }

    public void TryDeactivateHenshin()
    {
        if (henshinState != HenshinState.Giant) return;
        ToggleHenshin(HenshinState.Tank);
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
            tankCamera.transform.rotation = giantCamera.transform.rotation;
            enBar.gameObject.SetActive(true);
            enTimerBar.gameObject.SetActive(false);
            FindObjectOfType<KaijuTv>().OnGiantLeave();
        }
        else if (newState == HenshinState.Giant && henshinState != HenshinState.Giant)
        {
            giantInput.enabled = false;
            giantTransform.position = tankTransform.position;
            giantTransform.rotation = tankTransform.rotation;
            giantCamera.transform.position = tankCamera.transform.position;
            giantCamera.transform.rotation = tankCamera.transform.rotation;
            enBar.gameObject.SetActive(false);
            enTimerBar.gameObject.SetActive(true);
            giantTransform.DOScale(Vector3.one, 3f)
                .From(Vector3.one * 0.1f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => { giantInput.enabled = true; });

            FindObjectOfType<KaijuTv>().OnGiantEnter();
        }

        henshinState = newState;

        HenshinChanged?.Invoke(henshinState == HenshinState.Giant);
    }
}