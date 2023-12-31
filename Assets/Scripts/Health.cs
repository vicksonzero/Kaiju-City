using System;
using System.Collections;
using System.Collections.Generic;
using EditorCools;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public float hp = 100;
    public float hpMax = 100;
    public Transform bleedPrefab; // TODO: change to a bleed behaviour, and make it instantiate the bleed stuff

    public Transform healEffectPrefab;

    public bool canCollideBullets = true;
    public bool canTakeDamage = true;
    public bool canDie = true;
    public Transform autoAimRoot;

    public Bars[] bars;

    public float Percentage => hp / hpMax;


    public delegate void OnHealthUpdated(float hp, float hpMax);

    public OnHealthUpdated HealthUpdated;


    public delegate void OnDamageTaken(float hp, float hpMax, Vector3? hitPoint, Vector3? hitNormal,
        Vector3? hitImpulse);

    public OnDamageTaken DamageTaken;


    public void TakeDamage(float amount) => TakeDamage(amount, null, null, null);

    public void TakeDamage(float amount, Transform other)
    {
        var p = other.position;
        var n = -other.forward;
        TakeDamage(amount, p, n, transform.position - other.position);
    }

    // TODO: change amount into a data object with different damage attributes
    public void TakeDamage(float amount, Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse)
    {
        if (!canTakeDamage) return;
        hp -= amount;
        // Debug.Log($"TakeDamage {name} {amount} hp={hp}");

        HealthUpdated?.Invoke(hp, hpMax);

        // spawn effects
        // TODO: should be personal to the enemy/tank/jet
        if (bleedPrefab)
            Instantiate(bleedPrefab,
                hitPoint ?? transform.position,
                Quaternion.LookRotation(hitNormal ?? transform.forward, Vector3.up),
                ShouldDie() ? null : transform);

        UpdateBars();

        DamageTaken?.Invoke(hp, hpMax, hitPoint, hitNormal, hitImpulse);

        if (ShouldDie())
        {
            Die(hitPoint, hitNormal, hitImpulse);
        }
    }

    public bool ShouldDie() => (hp <= 0 && canDie);

    public void Heal(float amount)
    {
        var neededHealing = hp < hpMax;
        hp = Mathf.Min(hp + amount, hpMax);

        HealthUpdated?.Invoke(hp, hpMax);
        if (neededHealing && healEffectPrefab)
            Instantiate(healEffectPrefab,
                transform.position,
                Quaternion.identity,
                transform);

        UpdateBars();
    }

    [Button("Die (Play Mode)")]
    public void DieInPlayMode()
    {
        if (Application.isPlaying)
        {
            Debug.Log($"Health.DieInPlayMode('{name}')");
            Die(null, null, null);
        }
        else
        {
            Debug.Log("Health.DieInPlayMode() can only be used in play mode, duh.");
        }
    }

    private void Die(Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse)
    {
        var deathHandlers = GetComponents<ADeathHandler>();
        foreach (var deathHandler in deathHandlers)
        {
            deathHandler.Die(hitPoint, hitNormal, hitImpulse);
        }

        canDie = false;
        Destroy(gameObject);
    }

    public void UpdateBars()
    {
        foreach (var bar in bars)
        {
            bar.SetValue(hp, hpMax);
        }
    }
}