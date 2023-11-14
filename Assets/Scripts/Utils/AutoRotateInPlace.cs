using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateInPlace : MonoBehaviour
{
    public Transform target;

    public Vector3 rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        target.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}