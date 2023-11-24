using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{
    public GameObject prefab;

    public Vector3 offset;

    // public bool relativePosition = true;
    //
    // public bool copyRotation = true;

    private void OnDestroy()
    {
        if (!prefab) return;
        var forward = transform.forward;
        var relativeOffset = new Vector3(
            forward.x * offset.x,
            forward.y * offset.y,
            forward.z * offset.z);
        Instantiate(prefab, transform.position + relativeOffset, transform.rotation);
    }
}