using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public ParticleSystem damageTransitionPs;
    public bool damagedEffectDone;
    public Material damagedMaterial;
    public ParticleSystem damagedPs;
    public AudioSource damagedSfx;

    public bool veryDamagedEffectDone;
    public Material veryDamagedMaterial;
    public ParticleSystem veryDamagedPs;
    public AudioSource veryDamagedSfx;
    private Health _health;

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
        footprint.localScale = new Vector3(cube.localScale.x * 0.95f, footprintHeight, cube.localScale.z * 0.95f);
        footprint.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        cube.name = $"{name} Cube";

        _health = GetComponent<Health>();
        if (_health)
        {
            _health.HealthUpdated += OnHealthUpdated;
        }

        damageTransitionPs.Stop();
        damagedPs.Stop();
        veryDamagedPs.Stop();
    }

    public void OnHealthUpdated(float hp, float hpMax)
    {
        if (!damagedEffectDone && _health.Percentage <= 0.5f)
        {
            DOVirtual.DelayedCall(Random.Range(0, 0.5f), () =>
            {
                damageTransitionPs.Play();
                damagedSfx.Play();
            }, false);
            damagedPs.Play();
            var cubeRenderer = cube.GetComponent<MeshRenderer>();
            cubeRenderer.material = damagedMaterial;
            damagedEffectDone = true;
        }

        if (!veryDamagedEffectDone && _health.Percentage <= 0.2f)
        {
            DOVirtual.DelayedCall(Random.Range(0, 0.5f), () =>
            {
                damageTransitionPs.Play();
                veryDamagedSfx.Play();
            }, false);
            veryDamagedPs.Play();
            var cubeRenderer = cube.GetComponent<MeshRenderer>();
            cubeRenderer.material = damagedMaterial;
            veryDamagedEffectDone = true;
        }
    }
}