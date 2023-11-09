using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask explodeAtLayers;
    public LayerMask damageTheseLayers;
    public Transform explosionPrefab;

    public float kineticDamage;

    private Rigidbody _rb;
    private Vector3 hitPointCandidate;
    private Vector3 hitNormalCandidate;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var hasHit = Physics.Raycast(transform.position, transform.forward,
            out var hitInfo, _rb.velocity.magnitude, explodeAtLayers);
        hitPointCandidate = (hasHit ? hitInfo.point : transform.position);
        hitNormalCandidate = hasHit ? hitInfo.normal : -transform.forward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(hitPointCandidate, hitPointCandidate + hitNormalCandidate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (explodeAtLayers.Contains(other.gameObject.layer))
        {
            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(kineticDamage);
            }

            // if (pierce) { ... }
            if (explosionPrefab)
            {
                // var closestPoint = other.ClosestPointOnBounds(transform.position);
                // var collisionNormal = transform.position - closestPoint;
                Instantiate(
                    explosionPrefab,
                    hitPointCandidate,
                    // Quaternion.FromToRotation(Vector3.forward, Vector3.up) *
                    Quaternion.LookRotation(hitNormalCandidate, Vector3.up));
            }

            Destroy(gameObject);
        }
    }
}