using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public CinemachineVirtualCamera aimingCamera;


    [Header("Crosshairs")]
    public RectTransform basicCrosshair;

    public RectTransform cannonCrosshair;
    public RectTransform gunCrosshair;
    public RectTransform plasmaCrosshair;
    public RectTransform beamCrosshair;
    public RectTransform selectCrosshair;

    private StarterAssetsInputs _starterAssetsInputs;


    private void Awake()
    {
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_starterAssetsInputs.aim)
        {
            aimingCamera.gameObject.SetActive(true);
            UseCrosshair(CrosshairType.CannonCrosshair);
        }
        else
        {
            aimingCamera.gameObject.SetActive(false);
            UseCrosshair(CrosshairType.BasicCrosshair);
        }
    }

    void UseCrosshair(CrosshairType newCrosshair)
    {
        if (basicCrosshair) basicCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.BasicCrosshair);
        if (cannonCrosshair) cannonCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.CannonCrosshair);
        if (gunCrosshair) gunCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.GunCrosshair);
        if (plasmaCrosshair) plasmaCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.PlasmaCrosshair);
        if (beamCrosshair) beamCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.BeamCrosshair);
        if (selectCrosshair) selectCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.SelectCrosshair);
    }

    public enum CrosshairType
    {
        BasicCrosshair,
        CannonCrosshair,
        GunCrosshair,
        PlasmaCrosshair,
        BeamCrosshair,
        SelectCrosshair,
    }
}