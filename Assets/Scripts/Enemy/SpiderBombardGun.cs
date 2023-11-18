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
    public float landingRadius = 2f;
    public float kineticDamage = 10;

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
        ShootBullet();
        DOVirtual.DelayedCall(burstInterval, ShootBullet).SetLoops(burstCount - 1);
    }

    void ShootBullet()
    {
        var bullet = Instantiate(bulletPrefab, turretMuzzle.position, turretMuzzle.rotation, bulletDisplayList);
        var rb = bullet.GetComponent<Rigidbody>();
        bullet.kineticDamage = kineticDamage;
        var random = Random.insideUnitCircle * landingRadius;
        bullet.Play(target.position + new Vector3(random.x, 0, random.y));
        bullet.bulletDisplayList = bulletDisplayList;
        bullet.effectDisplayList = effectDisplayList;
    }
}