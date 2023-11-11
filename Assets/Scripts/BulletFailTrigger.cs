using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFailTrigger : MonoBehaviour
{
    [Header("Auto reference")]
    public LayerMask failAtLayers;
    public Action ParentHitFail;
    
    private void OnTriggerEnter(Collider other)
    {
        if (failAtLayers.Contains(other.gameObject.layer))
        {
            ParentHitFail();
        }
    }
}
