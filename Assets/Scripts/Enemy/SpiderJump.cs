using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EditorCools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpiderJump : MonoBehaviour
{
    public float cooldown = 20;
    public float height = 40;
    public float lookAtTargetPositionTime = 1;
    public float airTime = 5;
    public SphereCollider jumpRange;
    public LayerMask groundLayers;

    public Transform jumpMarker;
    public Transform jumpMarkerNewParent;

    public Transform lookAtTransform;

    // public DecalProjector jumpMarkerDecal;
    public ParticleSystem jumpPs;

    [Header("Shockwave")]
    public ParticleSystem shockwavePs;
    
    public float kineticDamage = 10;
    public LayerMask damageTheseLayers;

    public Bullet bulletPrefab;
    public Bullet buildingBulletPrefab;

    public Transform bulletDisplayList;
    public Transform effectDisplayList;

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        jumpMarker.gameObject.SetActive(false);
    }

    private void Start()
    {
        jumpMarker.SetParent(jumpMarkerNewParent);
        DOVirtual.DelayedCall(cooldown, Jump).SetLoops(-1);

        jumpPs.Stop();
    }

    [Button()]
    public void Jump() => JumpRandom();

    public void JumpRandom()
    {
        var offset = Random.insideUnitCircle * jumpRange.radius;
        var airPosition = jumpRange.transform.position + new Vector3(offset.x, 0, offset.y);
        airPosition.y += height;

        var hasHit = Physics.Raycast(airPosition, Vector3.down,
            out var hitInfo,
            height * 2, groundLayers);
        if (!hasHit)
        {
            Debug.LogWarning($"{nameof(SpiderJump)}.{nameof(JumpRandom)} cannot find a suitable place to land");
            return;
        }

        JumpTo(hitInfo.point, FindObjectOfType<Player>(false).transform);
    }

    public void JumpTo(Vector3 pos, Transform target)
    {
        lookAtTransform = target;
        startingPosition = transform.position;
        targetPosition = pos;
        jumpMarker.gameObject.SetActive(true);
        jumpMarker.position = targetPosition;
        jumpMarker.rotation = Quaternion.identity;

        var midPoint = (targetPosition + startingPosition) / 2;
        midPoint.y = startingPosition.y + height;

        var seq = DOTween.Sequence();

        seq.Append(transform.DOLookAt(targetPosition, lookAtTargetPositionTime,
            AxisConstraint.Y));
        seq.AppendCallback(() => jumpPs.Play());
        seq.Join(transform.DOMoveX(midPoint.x, airTime / 2f)
            .SetOptions(AxisConstraint.X)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveZ(midPoint.z, airTime / 2f)
            .SetOptions(AxisConstraint.Z)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveY(midPoint.y, airTime / 2f)
            .SetOptions(AxisConstraint.Y)
            .SetEase(Ease.OutCubic));
        
        seq.AppendCallback(() => jumpPs.Stop());
        seq.Join(DOVirtual.DelayedCall(airTime * 0.5f * 2f / 3f,
            () => jumpMarker.gameObject.SetActive(false)));
        seq.Join(transform.DOMoveX(targetPosition.x, airTime / 2f)
            .SetOptions(AxisConstraint.X)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveZ(targetPosition.z, airTime / 2f)
            .SetOptions(AxisConstraint.Z)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveY(targetPosition.y, airTime / 2f)
            .SetOptions(AxisConstraint.Y)
            .SetEase(Ease.InCubic));
        seq.Join(DOVirtual.Float(0f, 1f, airTime * 0.5f * 2f / 3f,
            value =>
            {
                if (!lookAtTransform.gameObject.activeInHierarchy)
                {
                    lookAtTransform = FindObjectOfType<Player>(false).transform;
                }

                if (!lookAtTransform) return;

                var dirVector = lookAtTransform.position - transform.position;
                dirVector.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(
                        dirVector, Vector3.up),
                    value
                );
            }));
        seq.AppendInterval(0.4f);
        seq.AppendCallback(SpawnShockwave);

        seq.Play();
    }

    public void SpawnShockwave()
    {
        // Debug.Log($"{nameof(SpiderJump)}.{nameof(SpawnShockwave)}");

        ShootBullet();
        ShootBuildingBullet();
        shockwavePs.Play();
    }
    
    void ShootBullet()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletDisplayList);
        bullet.kineticDamage = kineticDamage;
        bullet.damageTheseLayers = damageTheseLayers;
        bullet.effectDisplayList = effectDisplayList;
    }
    
    void ShootBuildingBullet()
    {
        var bullet = Instantiate(buildingBulletPrefab, transform.position, Quaternion.identity, bulletDisplayList);
    }
}