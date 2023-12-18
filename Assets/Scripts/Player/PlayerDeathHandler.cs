using System;
using Cinemachine;
using UnityEngine;

public class PlayerDeathHandler : ADeathHandler
{
    public Transform deathAnimationPrefab;

    private Transform _effectDisplayList;

    public CinemachineVirtualCamera playerVirtualCamera;

    private void Start()
    {
        _effectDisplayList = DisplayListRepository.Inst.effectDisplayList;
    }

    public override void Die(Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse)
    {
        playerVirtualCamera.gameObject.SetActive(false);
        ;
        var anim = Instantiate(
            deathAnimationPrefab,
            transform.position,
            transform.rotation,
            _effectDisplayList);
        if (hitImpulse.HasValue && hitPoint.HasValue && anim.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForceAtPosition(hitImpulse.Value * 0.2f, hitPoint.Value, ForceMode.Impulse);
        }
    }
}