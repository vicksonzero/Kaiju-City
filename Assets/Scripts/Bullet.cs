using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask explodeAtLayers;
    public Transform explosionPrefab;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (explodeAtLayers.Contains(col.gameObject.layer))
        {
            // if (other.gameObject.TryGetComponent(out Health health)){
            // health.takeDamage();
            // }
            // if (pierce) { ... }
            if (explosionPrefab)
            {
                // var closestPoint = other.ClosestPointOnBounds(transform.position);
                // var collisionNormal = transform.position - closestPoint;
                Instantiate(
                    explosionPrefab,
                    col.contacts[0].point,
                    // Quaternion.FromToRotation(Vector3.forward, Vector3.up) *
                    Quaternion.LookRotation(col.contacts[0].normal, Vector3.up));
            }

            Destroy(gameObject);
        }
    }
}