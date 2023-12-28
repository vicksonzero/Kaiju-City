using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistingGizmoLine : MonoBehaviour
{
    public Vector3 direction;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var position = transform.position;
        Gizmos.DrawSphere(position, 1);
        Gizmos.DrawLine(position, position + direction);
    }
}