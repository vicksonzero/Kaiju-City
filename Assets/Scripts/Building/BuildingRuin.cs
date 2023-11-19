using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuildingRuin : MonoBehaviour
{
    public Transform sinkingRoot;
    public Transform cube;
    public ParticleSystem effectPS;

    public float sinkHeight;
    public float sinkTime;
    public Vector3 maxRotate;

    public void Init(Vector3 scale)
    {
        cube.localScale = scale;
        cube.localPosition = new Vector3(0, -scale.y / 2, 0);
        sinkingRoot.localPosition = new Vector3(0, scale.y, 0);
    }

    public void Play()
    {
        effectPS.Play();
        var seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.Append(sinkingRoot.DORotate(new Vector3(
            Random.Range(-maxRotate.x, maxRotate.x),
            Random.Range(-maxRotate.y, maxRotate.y),
            Random.Range(-maxRotate.z, maxRotate.z)
        ), sinkTime).SetEase(Ease.InOutQuint));
        seq.Join(sinkingRoot.DOMoveY(sinkHeight, sinkTime)
            .SetEase(Ease.InOutQuint)
            .OnComplete(() => { effectPS.Stop(); }));
        seq.Play();
    }
}