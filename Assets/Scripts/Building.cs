using System;
using System.Collections;
using System.Collections.Generic;
using EditorCools;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class Building : MonoBehaviour
{
    public Transform cube;
    public Vector3 minDimension;
    public Vector3 maxDimension;
    public float precision = 0.5f;

    [Button("Randomize Building")]
    // ReSharper disable once UnusedMember.Local
    void RandomizeBuilding()
    {
        var scale = new Vector3(
            Mathf.Floor(Random.Range(minDimension.x, maxDimension.x) / precision) * precision,
            Mathf.Floor(Random.Range(minDimension.y, maxDimension.y) / precision) * precision,
            Mathf.Floor(Random.Range(minDimension.z, maxDimension.z) / precision) * precision
        );
        print($"RandomizeBuilding: {scale}");
        cube.localScale = scale;
        cube.localPosition = new Vector3(0, scale.y / 2, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}