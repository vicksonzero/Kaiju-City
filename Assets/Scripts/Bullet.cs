using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [FormerlySerializedAs("explodeAtLayers")]
    public LayerMask failAtLayers;

    public LayerMask damageTheseLayers;
    public Transform explosionPrefab;
    public BulletFailTrigger failCollider;

    public float kineticDamage;

    private Rigidbody _rb;
    private Vector3? hitPointCandidate;
    private Vector3? hitNormalCandidate;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (!failCollider) failCollider = GetComponentInChildren<BulletFailTrigger>();
        failCollider.ParentHitFail = OnHitFail;
        failCollider.failAtLayers = failAtLayers;
    }

    private void FixedUpdate()
    {
        var hasHit = Physics.Raycast(transform.position, transform.forward,
            out var hitInfo, _rb.velocity.magnitude, failAtLayers);
        hitPointCandidate = (hasHit ? hitInfo.point : transform.position);
        hitNormalCandidate = hasHit ? hitInfo.normal : -transform.forward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var p = hitPointCandidate ?? transform.position;
        var n = hitNormalCandidate ?? -transform.forward;
        Gizmos.DrawLine(p, p + n);
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponentInParent<Health>();
        if (health && damageTheseLayers.Contains(health.gameObject.layer))
        {
            OnHitSuccess(health);
        }
    }

    private void OnHitFail()
    {
        if (explosionPrefab)
        {
            var p = hitPointCandidate ?? transform.position;
            var n = hitNormalCandidate ?? -transform.forward;
            // var closestPoint = other.ClosestPointOnBounds(transform.position);
            // var collisionNormal = transform.position - closestPoint;
            Instantiate(
                explosionPrefab,
                p,
                // Quaternion.FromToRotation(Vector3.forward, Vector3.up) *
                Quaternion.LookRotation(n, Vector3.up));
        }

        Destroy(gameObject);
    }

    private void OnHitSuccess(Health health)
    {
        health.TakeDamage(kineticDamage);

        // if (pierce) { ... }
        if (explosionPrefab)
        {
            var p = hitPointCandidate ?? transform.position;
            var n = hitNormalCandidate ?? -transform.forward;
            // var closestPoint = other.ClosestPointOnBounds(transform.position);
            // var collisionNormal = transform.position - closestPoint;
            Instantiate(
                explosionPrefab,
                p,
                // Quaternion.FromToRotation(Vector3.forward, Vector3.up) *
                Quaternion.LookRotation(n, Vector3.up));
        }
    }
}