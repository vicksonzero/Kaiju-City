using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDeathSequence : MonoBehaviour
{
    public float delay;

    public Transform explosionPrefab;
    public Transform chasis;
    public float shakeDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.DelayedCall(0.2f, () =>
            chasis.DOLocalMove(Random.insideUnitSphere * shakeDistance, 0.2f)
                .SetLoops(-1)
        );
        DOVirtual.DelayedCall(delay, () =>
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            FindObjectOfType<ArcadeObjective>().OnPlayerDeathAnimationFinished();
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        DOTween.Kill(chasis);
    }
}