using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class SpiderDeathHandler : ADeathHandler
{
    public Transform deathAnimationPrefab;
    public CinemachineTargetGroup newsVCamTargetGroup;

    private Transform _effectDisplayList;

    private void Start()
    {
        _effectDisplayList = DisplayListRepository.Inst.effectDisplayList;
    }

    public override void Die(Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse)
    {
        var anim = Instantiate(
            deathAnimationPrefab,
            transform.position,
            transform.rotation,
            _effectDisplayList);

        newsVCamTargetGroup.m_Targets = newsVCamTargetGroup.m_Targets
            .Concat(new[]
            {
                new CinemachineTargetGroup.Target()
                {
                    target = anim.GetChild(0),
                    weight = 100f,
                    radius = 8f,
                }
            }).ToArray();
        
        FindObjectOfType<KaijuTv>().OnBossDie();
    }
}