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
    public Transform footprint;
    public Vector3 minDimension;
    public Vector3 maxDimension;
    public float footprintHeight = 0.1f;
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
        footprint.localScale = new Vector3(scale.x, footprintHeight, scale.z);
        footprint.localPosition = Vector3.zero;
    }

    private void OnValidate()
    {
        footprint.localScale = new Vector3(cube.localScale.x*0.95f, footprintHeight, cube.localScale.z*0.95f);
        footprint.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        cube.name = $"{name} Cube";
    }

    // Update is called once per frame
    void Update()
    {
    }
}