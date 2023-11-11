using UnityEngine;
using DG.Tweening;
using EditorCools;
using JetBrains.Annotations;
using UnityEngine.Serialization;

public class WaspEnemyGun : MonoBehaviour
{
    public delegate void OnChargeComplete();

    public OnChargeComplete ChargeCompleted;

    public float chargeTime = 1.2f;

    public float chargeTimer = 0;

    public int burstCount = 1;
    public float burstInterval = 0.3f;
    public float bulletSpeed = 2f;
    public float kineticDamage = 10;

    [Header("Reference")]
    public Transform target;

    public Transform chargeBall;

    [FormerlySerializedAs("chargeParticles")]
    public ParticleSystem chargePs;

    public Bullet bulletPrefab;
    public Transform turretMuzzle;

    private Tween _ballTween;

    [CanBeNull]
    private Tween _burstFireTween;

    // Start is called before the first frame update
    void Start()
    {
        StopCharging();
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeTimer < 0) return;
        turretMuzzle.LookAt(target, transform.up);
        if (chargeTimer < chargeTime)
        {
            chargeTimer += Time.deltaTime;
        }
        else
        {
            StopCharging();
            _burstFireTween?.Kill();
            ShootCannonBullet();
            _burstFireTween = DOVirtual.DelayedCall(burstInterval, ShootCannonBullet)
                .SetLoops(burstCount - 1);
            // ChargeCompleted?.Invoke();
        }
    }

    [Button()]
    public void StartCharging()
    {
        chargeTimer = 0;
        _ballTween = chargeBall.DOScale(Vector3.one, chargeTime).SetEase(Ease.InCubic);
        chargePs.Play(true);
    }

    public void StopCharging()
    {
        _ballTween.Kill();
        _burstFireTween?.Kill();
        chargeTimer = -1;
        chargeBall.localScale = Vector3.zero;
        chargePs.Stop(true);
    }

    private void ShootCannonBullet()
    {
        Debug.Log("ShootCannonBullet");
        var bullet = Instantiate(bulletPrefab, turretMuzzle.position, turretMuzzle.rotation);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;
        bullet.kineticDamage = kineticDamage;
    }

    private void OnDestroy()
    {
        _ballTween.Kill();
        _burstFireTween?.Kill();
    }
}