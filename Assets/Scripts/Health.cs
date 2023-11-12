using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public float hp = 100;
    public float hpMax = 100;
    public Transform bleedPrefab; // TODO: change to a bleed behaviour, and make it instantiate the bleed stuff

    public Transform deathAnimationPrefab;
    public Transform effectDisplayList;

    public bool canDie = true;

    public Bars[] bars;


    // private Death _death; // TODO: some entities can take damage, but not die?

    private void Start()
    {
        effectDisplayList = DisplayListRepository.Inst.effectDisplayList;
    }

    public void TakeDamage(float amount) => TakeDamage(amount, null, null);

    public void TakeDamage(float amount, Transform other)
    {
        var p = other.position;
        var n = -other.forward;
        TakeDamage(amount, p, n);
    }

    // TODO: change amount into a data object with different damage attributes
    public void TakeDamage(float amount, Vector3? hitPoint, Vector3? hitNormal)
    {
        hp -= amount;
        // Debug.Log($"TakeDamage {name} {amount} hp={hp}");

        // spawn effects
        // TODO: should be personal to the enemy/tank/jet
        if (bleedPrefab)
            Instantiate(bleedPrefab,
                hitPoint ?? transform.position,
                Quaternion.LookRotation(hitNormal ?? transform.forward, Vector3.up),
                effectDisplayList);

        foreach (var bar in bars)
        {
            bar.SetValue(hp, hpMax);
        }

        if (hp <= 0 && canDie)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Min(hp + amount, hpMax);
    }

    private void Die()
    {
        Instantiate(deathAnimationPrefab, transform.position, transform.rotation, effectDisplayList);
        canDie = false;
        Destroy(gameObject);
    }
}