using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RepairPack : MonoBehaviour
{
    public float healAmount = 30;
    public Transform pickedUpEffect;

    public void OnTriggerEnter(Collider col)
    {
        // Debug.Log("OnTriggerEnter");
        var player = col.gameObject.GetComponent<Player>();
        if (player)
        {
            // Debug.Log("player");
            var health = player.GetComponent<Health>();
            if (!health) return;
            health.Heal(healAmount);

            if (pickedUpEffect)
            {
                Instantiate(pickedUpEffect, transform.position, Quaternion.identity);
                Instantiate(pickedUpEffect, player.transform);
            }

            Destroy(gameObject);
        }
    }
}