using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public CinemachineVirtualCamera aimingCamera;
    public CrosshairType startingCrosshairType;

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

    private void Start()
    {
        UseCrosshair(startingCrosshairType);
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

public class CannonWeapon : MonoBehaviour
{
    public LayerMask canHitLayers;
    public float aimMaxDistance = 1000f;

    /// <summary>
    /// Rotate the cannon with this
    /// </summary>
    public Transform turretBase;

    /// <summary>
    /// Pitch the cannon with this
    /// </summary>
    public Transform turretBarrel;

    /// <summary>
    /// Bullets spawn from here
    /// </summary>
    public Transform turretMuzzle;

    /// <summary>
    /// This point stores the current aim after raycast with screenRay
    /// </summary>
    public Transform targetPoint;

    private PlayerAiming _playerAiming;

    private void Start()
    {
        _playerAiming = GetComponent<PlayerAiming>();
    }

    public void Aim(Ray screenRay)
    {
        UpdateTargetPoint(screenRay);
        UpdateTurret();
    }

    public void UpdateTargetPoint(Ray screenRay)
    {
        var isHit = Physics.Raycast(screenRay, out var hitInfo, aimMaxDistance, canHitLayers);

        if (!isHit)
        {
            targetPoint.transform.position = screenRay.GetPoint(aimMaxDistance);
        }
        else
        {
            targetPoint.transform.position = hitInfo.point;
        }
    }

    public void UpdateTurret()
    {
        turretBase.rotation
        turretBarrel.rotation
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        _playerAiming.UseCrosshair(CrosshairType.CannonCrosshair);
    }
}