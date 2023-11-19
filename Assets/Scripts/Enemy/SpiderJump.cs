using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EditorCools;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpiderJump : MonoBehaviour
{
    public float cooldown = 20;
    public float height = 40;
    public float airTime = 5;
    public SphereCollider jumpRange;
    public LayerMask groundLayers;

    public Transform jumpMarker;
    public Transform jumpMarkerNewParent;

    public Transform lookAtTransform;

    // public DecalProjector jumpMarkerDecal;
    public ParticleSystem jumpPs;

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        jumpMarker.SetParent(jumpMarkerNewParent);
        jumpMarker.gameObject.SetActive(false);
        DOVirtual.DelayedCall(cooldown, Jump).SetLoops(-1);
        
        jumpPs.Stop();
    }

    [Button()]
    public void Jump() => JumpRandom();

    public void JumpRandom()
    {
        var offset = Random.insideUnitCircle * jumpRange.radius;
        var airPosition = jumpRange.transform.position + new Vector3(offset.x, 0, offset.y);
        airPosition.y += height;

        var hasHit = Physics.Raycast(airPosition, Vector3.down,
            out var hitInfo,
            height * 2, groundLayers);
        if (!hasHit)
        {
            Debug.LogWarning($"{nameof(SpiderJump)}.{nameof(JumpRandom)} cannot find a suitable place to land");
            return;
        }

        JumpTo(hitInfo.point);
    }

    public void JumpTo(Vector3 pos)
    {
        startingPosition = transform.position;
        targetPosition = pos;
        jumpMarker.gameObject.SetActive(true);
        jumpMarker.position = targetPosition;
        jumpMarker.rotation = Quaternion.identity;

        var midPoint = (targetPosition + startingPosition) / 2;
        midPoint.y = startingPosition.y + height;


        var seq = DOTween.Sequence();
        seq.Append(transform.DOMoveX(midPoint.x, airTime / 2f)
            .SetOptions(AxisConstraint.X)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveZ(midPoint.z, airTime / 2f)
            .SetOptions(AxisConstraint.Z)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveY(midPoint.y, airTime / 2f)
            .SetOptions(AxisConstraint.Y)
            .SetEase(Ease.OutCubic));
        seq.Append(transform.DOMoveX(targetPosition.x, airTime / 2f)
            .SetOptions(AxisConstraint.X)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveZ(targetPosition.z, airTime / 2f)
            .SetOptions(AxisConstraint.Z)
            .SetEase(Ease.Linear));
        seq.Join(transform.DOMoveY(targetPosition.y, airTime / 2f)
            .SetOptions(AxisConstraint.Y)
            .SetEase(Ease.InCubic));
        // seq.Join();
        seq.AppendCallback(() => jumpPs.Stop());
        seq.AppendInterval(0.4f);
        seq.AppendCallback(SpawnShockwave);
        seq.AppendCallback(() => jumpMarker.gameObject.SetActive(false));

        seq.Play();
        jumpPs.Play();
    }

    public void SpawnShockwave()
    {
        Debug.Log($"{nameof(SpiderJump)}.{nameof(SpawnShockwave)}");
    }
}