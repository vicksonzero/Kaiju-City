using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public Bullet bulletPrefab;
    public Transform bulletDisplayList;

    public float kineticDamage = 1;
    public float bulletSpeed = 10;
    public float rapid = 1.2f;
    public float nextBulletReady = 0;
    public float ammo = 100;
    public float ammoMax = 100;

    public float turretReactionDistance = 0.2f;
    public ParticleSystem muzzlePs;

    public float cameraShakeForce = 0.2f;
    public CinemachineImpulseSource cinemachineImpulseSource;
    public AudioSource audioSource;
    public AudioClip[] sfxList;
    public float sfxVolume;

    private PlayerAiming _playerAiming;

    private void Start()
    {
        _playerAiming = GetComponent<PlayerAiming>();
        bulletDisplayList = DisplayListRepository.Inst.bulletDisplayList;
    }

    public void Aim(Ray screenRay)
    {
        UpdateTargetPoint(screenRay);
        UpdateTurret();
    }

    public void TryShoot()
    {
        if (Time.time > nextBulletReady)
        {
            ShootCannonBullet();
            nextBulletReady = Time.time + rapid;
        }
    }

    private void UpdateTargetPoint(Ray screenRay)
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

    private void UpdateTurret()
    {
        var turretDisplacement = transform.InverseTransformPoint(targetPoint.position);
        var horizontalLocalRotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(turretDisplacement, Vector3.up),
            Vector3.up);
        turretBase.localRotation =
            Quaternion.RotateTowards(turretBase.localRotation, horizontalLocalRotation,
                turretRotationSpeed * Time.deltaTime);

        var barrelDisplacement = targetPoint.position - turretBarrel.position;
        var verticalAngle = Vector3.SignedAngle(turretBase.forward, barrelDisplacement, turretBase.right);
        // Debug.Log($"VertAngle {verticalAngle}");
        // Debug.Log($"VertAngle {turretBarrel}");
        var verticalLocalRotation = Quaternion.Euler(verticalAngle, 0, 0);
        turretBarrel.localRotation = Quaternion.RotateTowards(turretBarrel.localRotation, verticalLocalRotation,
            turretRotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(turretBase.position, turretBase.position + turretBase.forward * 100f);
        var turretDisplacement = turretBase.InverseTransformPoint(targetPoint.position);
        Gizmos.DrawLine(turretBase.position,
            turretBase.position + Vector3.ProjectOnPlane(turretDisplacement, Vector3.up) * 100f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(turretBarrel.position, turretBarrel.position + turretBarrel.forward * 100f);
    }

    private void ShootCannonBullet()
    {
        // Debug.Log("ShootCannonBullet");
        var bullet = Instantiate(bulletPrefab, turretMuzzle.position, turretMuzzle.rotation, bulletDisplayList);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;
        bullet.kineticDamage = kineticDamage;
        audioSource.PlayOneShot(sfxList[Random.Range(0, sfxList.Length)], sfxVolume);
        muzzlePs.Play();
        
        cinemachineImpulseSource.GenerateImpulse(cameraShakeForce);
        var turretBaseLocalForward = turretBase.parent.InverseTransformDirection(turretBase.forward);
        turretBase.DOPunchPosition(turretBaseLocalForward * -turretReactionDistance, 0.2f);
    }
}