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


    // private Death _death; // TODO: some entities can take damage, but not die?


    public void TakeDamage(float amount) => TakeDamage(amount, null, null);

    // TODO: change amount into a data object with different damage attributes
    public void TakeDamage(float amount, Vector3? hitPoint, Vector3? hitNormal)
    {
        hp -= amount;

        // spawn effects
        // TODO: should be personal to the enemy/tank/jet
        if (bleedPrefab)
            Instantiate(bleedPrefab,
                hitPoint ?? transform.position,
                Quaternion.LookRotation(hitNormal ?? transform.forward, Vector3.up));

        if (hp <= 0)
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
        Instantiate(deathAnimationPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}