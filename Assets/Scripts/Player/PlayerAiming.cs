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
    // public RectTransform gunCrosshair;
    // public RectTransform plasmaCrosshair;
    // public RectTransform beamCrosshair;
    // public RectTransform selectCrosshair;

    private StarterAssetsInputs _input;
    private ThirdPersonTankController _controller;

    private CannonWeapon[] _weapons;


    private void Awake()
    {
        _input = FindObjectOfType<StarterAssetsInputs>();
        _controller = GetComponent<ThirdPersonTankController>();
    }

    private void Start()
    {
        UseCrosshair(startingCrosshairType);
        _weapons = GetComponents<CannonWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.Inst.isPaused) return;
        // if (_input.aim)
        if (false)
        {
            aimingCamera.gameObject.SetActive(true);
            UseCrosshair(CrosshairType.CannonCrosshair);

            var mainCamera = Camera.main;
            if (mainCamera)
            {
                var ray = mainCamera.ScreenPointToRay(new Vector3(
                    mainCamera.pixelWidth / 2f,
                    mainCamera.pixelHeight / 2f
                ));
                foreach (var weapon in _weapons)
                {
                    weapon.Aim(ray);
                }
            }
        }
        else
        {
            aimingCamera.gameObject.SetActive(false);
            UseCrosshair(CrosshairType.BasicCrosshair);
        }

        if (_controller.canControlMovement && _input.shoot && !GamePause.Inst.isPaused)
        {
            foreach (var weapon in _weapons)
            {
                weapon.TryShoot();
            }
        }
    }

    public void AimAtRay(Ray ray)
    {
        // Debug.Log("AimAtRay");
        foreach (var weapon in _weapons)
        {
            weapon.Aim(ray);
        }
    }

    public void UseCrosshair(CrosshairType newCrosshair)
    {
        if (basicCrosshair) basicCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.BasicCrosshair);
        if (cannonCrosshair) cannonCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.CannonCrosshair);
        // if (gunCrosshair) gunCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.GunCrosshair);
        // if (plasmaCrosshair) plasmaCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.PlasmaCrosshair);
        // if (beamCrosshair) beamCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.BeamCrosshair);
        // if (selectCrosshair) selectCrosshair.gameObject.SetActive(newCrosshair == CrosshairType.SelectCrosshair);
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