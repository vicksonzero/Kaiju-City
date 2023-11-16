using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingDeathHandler : ADeathHandler
{
    public BuildingRuin deathAnimationPrefab;

    private Transform _effectDisplayList;

    private void Start()
    {
        _effectDisplayList = DisplayListRepository.Inst.effectDisplayList;
    }

    public override void Die(Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse)
    {
        var building = GetComponent<Building>();
        var anim = Instantiate(
            deathAnimationPrefab,
            transform.position,
            transform.rotation,
            _effectDisplayList);
        anim.Init(building.cube.localScale);
        anim.Play();
    }
}