using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EditorCools;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderBombardGun : MonoBehaviour
{
    public float cooldown = 10f;
    public int burstCount = 8;
    public float burstInterval = 0.6f;
    public float height = 40;
    public float landingRadius = 2f;
    public float kineticDamage = 10;
    public LayerMask groundLayers;

    public Transform target;

    [Header("Reference")]
    public BombardBullet bulletPrefab;

    public Transform bulletDisplayList;
    public Transform effectDisplayList;

    public Transform turretMuzzle;


    private void Start()
    {
        DOVirtual.DelayedCall(cooldown, Shoot).SetLoops(-1);
    }

    [Button()]
    public void Shoot()
    {
        target = FindObjectOfType<Player>(false).transform;
        ShootBullet();
        DOVirtual.DelayedCall(burstInterval, ShootBullet).SetLoops(burstCount - 1);
    }

    void ShootBullet()
    {
        if (!target.gameObject.activeInHierarchy)
        {
            target = FindObjectOfType<Player>(false).transform;
        }
        
        var bullet = Instantiate(bulletPrefab, turretMuzzle.position, turretMuzzle.rotation, bulletDisplayList);
        var rb = bullet.GetComponent<Rigidbody>();
        bullet.kineticDamage = kineticDamage;
        
        var offset = Random.insideUnitCircle * landingRadius;
        var airPosition = target.position + new Vector3(offset.x, 0, offset.y);
        airPosition.y += height;

        var hasHit = Physics.Raycast(airPosition, Vector3.down,
            out var hitInfo,
            height * 2, groundLayers);
        bullet.Play(hitInfo.point);
        bullet.bulletDisplayList = bulletDisplayList;
        bullet.effectDisplayList = effectDisplayList;
    }
}