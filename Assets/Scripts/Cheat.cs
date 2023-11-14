using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public bool playerIsInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        if (playerIsInvincible)
            foreach (var player in FindObjectsOfType<Player>())
            {
                if (player.TryGetComponent(out Health health))
                {
                    health.canDie = false;
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
    }
}