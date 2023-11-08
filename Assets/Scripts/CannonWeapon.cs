using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWeapon : MonoBehaviour
{
    public LayerMask canHitLayers;
    public float aimMaxDistance = 1000f;
    public float turretRotationSpeed = 2f;

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
        var turretDisplacement = targetPoint.position - turretBase.position;
        var horizontalRotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(turretDisplacement, transform.up),
            transform.up);
        turretBase.rotation =
            Quaternion.Slerp(turretBase.rotation, horizontalRotation, turretRotationSpeed * Time.deltaTime);

        var barrelDisplacement = targetPoint.position - turretBarrel.position;
        var verticalAngle = Vector3.SignedAngle(turretBase.forward, barrelDisplacement, turretBase.right);
        var verticalRotation = Quaternion.AngleAxis(-verticalAngle, -turretBase.right);
        turretBarrel.localRotation = 
            Quaternion.Slerp(turretBarrel.localRotation, verticalRotation, turretRotationSpeed * Time.deltaTime);
           
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        _playerAiming.UseCrosshair(PlayerAiming.CrosshairType.CannonCrosshair);
    }
}