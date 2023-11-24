using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialsPack : MonoBehaviour
{
    public float energyAmount = 8;
    public Transform pickedUpEffect;

    public void OnTriggerEnter(Collider col)
    {
        // Debug.Log("OnTriggerEnter");
        var player = col.gameObject.GetComponent<Player>();
        if (player)
        {
            // Debug.Log("player");
            var henshin = FindObjectOfType<Henshin>();
            if (!henshin) return;
            henshin.AddEnergy(energyAmount);

            if (pickedUpEffect)
            {
                Instantiate(pickedUpEffect, player.transform);
            }

            Destroy(gameObject);
        }
    }
}