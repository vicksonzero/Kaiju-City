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
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Henshin : MonoBehaviour
{
    public float energy = 0;
    public float energyMax = 60;
    public float targetEnergy = 1f;

    [FormerlySerializedAs("henshinTimerDone")]
    public bool henshinRequirementsDone = false;

    public float henshinInvincibility = 7;

    public Bars enBar;
    public Bars enTimerBar;
    public Bars[] bars;
    public AudioSource audioSource;
    public AudioSource timeUpChime;
    public AudioClip henshinSfx;
    public AudioClip henshinCancelSfx;
    public AudioClip henshinReadySfx;
    public float sfxVolume = 1;

    public delegate void OnHenshinChanged(bool isGiant);

    public OnHenshinChanged HenshinChanged;

    public HenshinState henshinState;
    public Transform cameraFollowRoot;
    public float henshinButtonBigScale = 5;
    public RectTransform henshinButton;
    public float henshinPcLabelBigScale = 20;
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
        henshinButton.DOLocalRotate(Vector3.forward * 1, 0.1f)
            .From(Vector3.forward * -1)
            .SetLoops(-1);
        henshinPcLabel.DOLocalRotate(Vector3.forward * 1, 0.1f)
            .From(Vector3.forward * -1)
            .SetLoops(-1);

        enBar.gameObject.SetActive(true);
        enTimerBar.gameObject.SetActive(false);
        // DOVirtual.DelayedCall(90, () =>
        // {
        //     henshinRequirementsDone = true;
        //     TryShowHenshin();
        // }, false);


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

            if (energy <= 10 && !timeUpChime.isPlaying)
            {
                timeUpChime.Play();
            }
        }

        if (henshinState != HenshinState.Giant || energy > 10)
        {
            timeUpChime.Stop();
        }
    }

    [Button()]
    public void ForceShowHenshin()
    {
        AddEnergy(60);
        henshinRequirementsDone = true;
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
        if (!henshinRequirementsDone) return false;
        if (henshinState == HenshinState.Giant) return false;
        if (energy / energyMax < targetEnergy) return false;
        return true;
    }

    public bool TryShowHenshin()
    {
        if (henshinRequirementsDone) return false;
        if (henshinState == HenshinState.Giant) return false;
        if (energy / energyMax < targetEnergy) return false;

        audioSource.PlayOneShot(henshinReadySfx, sfxVolume);
        henshinRequirementsDone = true;
        targetEnergy = 0.5f;
#if (UNITY_IOS || UNITY_ANDROID)
        henshinButton.GetComponent<Button>().enabled = false;
        henshinButton.gameObject.SetActive(true);
        henshinButton.DOScale(Vector3.one, 0.3f)
            .From(Vector3.one * henshinButtonBigScale)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
                henshinButton.GetComponent<Button>().enabled = true);
#else
        henshinPcLabel.gameObject.SetActive(true);
        henshinPcLabel.DOScale(Vector3.one, 0.3f)
            .From(Vector3.one * henshinPcLabelBigScale)
            .SetEase(Ease.InCubic);
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
            audioSource.PlayOneShot(henshinCancelSfx, sfxVolume);

            henshinRequirementsDone = false;
            henshinButton.gameObject.SetActive(false);
            henshinPcLabel.gameObject.SetActive(false);
            
            FindObjectOfType<ArcadeObjective>().OnHenshinEnd();

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

            giantTransform.GetComponent<HealthInvincibility>().ApplyTimedInvincibility(henshinInvincibility);

            henshinButton.gameObject.SetActive(false);
            henshinPcLabel.gameObject.SetActive(false);
            
            FindObjectOfType<ArcadeObjective>().OnHenshinStart();

            audioSource.PlayOneShot(henshinSfx, sfxVolume);

            FindObjectOfType<KaijuTv>().OnGiantEnter();
        }

        henshinState = newState;

        HenshinChanged?.Invoke(henshinState == HenshinState.Giant);
    }
}