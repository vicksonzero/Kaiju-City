using System;
using UnityEngine;

public class BulletFailTrigger : MonoBehaviour
{
    [Header("Auto reference")]
    public LayerMask failAtLayers;

    public Action ParentHitFail;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"BulletFailTrigger {other.gameObject.name}, {other.gameObject.layer}");
        if (failAtLayers.Contains(other.gameObject.layer))
        {
            Debug.Log($"BulletFailTrigger");
            ParentHitFail();
        }
    }
}