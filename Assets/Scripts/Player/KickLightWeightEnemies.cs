using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickLightWeightEnemies : MonoBehaviour
{
    public float attackPower = 200;
    public float kickImpulse = 0.5f;
    public Transform hitEffectPrefab; // TODO: change to a bleed behaviour, and make it instantiate the bleed stuff


    private void OnTriggerEnter(Collider other)
    {
        var lightWeightEnemy = other.GetComponentInParent<LightWeightEnemy>();
        if (!lightWeightEnemy) return;

        var health = lightWeightEnemy.GetComponent<Health>();
        var hitPoint = other.ClosestPoint(transform.position);
        var p = hitPoint - transform.position;
        health.TakeDamage(attackPower, hitPoint, -p, p * kickImpulse);

        if (hitEffectPrefab)
            Instantiate(hitEffectPrefab,
                hitPoint,
                Quaternion.LookRotation(p, Vector3.up),
                transform);
    }
}