using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaspEnemyDeath : MonoBehaviour
{
    public float delay = 2f;

    public Transform explosionPrefab;
    public Transform effectDisplayList;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Die), delay);
        effectDisplayList = DisplayListRepository.Inst.effectDisplayList;
    }

    // Update is called once per frame
    void Die()
    {
        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity, effectDisplayList);
        Destroy(gameObject);
    }
}