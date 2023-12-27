using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpiderDeath : MonoBehaviour
{
    public Transform body;

    public float shakeDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.DelayedCall(
                0.1f,
                () => body.DOLocalMove(Random.insideUnitSphere * shakeDistance, 0.1f),
                false)
            .SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DestroySelf()
    {
        FindObjectOfType<ArcadeObjective>().OnBossDeathAnimationFinished();
        Destroy(gameObject);
    }
}