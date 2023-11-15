using System;
using UnityEngine;

public class BulletSuccessTrigger : MonoBehaviour
{
    [Header("Auto reference")]
    public LayerMask damageTheseLayers;
    public Action<Health, Collider> ParentHitSuccess;
    
    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponentInParent<Health>();
        if (health && damageTheseLayers.Contains(health.gameObject.layer))
        {
            // Debug.Log($"BulletSuccessTrigger");
            ParentHitSuccess(health, other);
        }
    }

}
