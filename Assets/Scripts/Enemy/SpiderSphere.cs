using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public class SpiderSphere : ADeathHandler
{
    public Transform sphere;
    public Transform sphereShell;
    public Health spiderHealth;

    public float damageToHost = 700;

    public Vector2 openInterval = new Vector2(10, 15);
    public Vector2 openDuration = new Vector2(5, 8);

    private Health _health;

    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
        Close();
    }

    void TryOpen()
    {
        if (FindObjectOfType<ThirdPersonTankController>(false))
        {
            Open();
            return;
        }

        DOVirtual.DelayedCall(Random.Range(openInterval.x, openInterval.y), TryOpen);
    }

    void Open()
    {
        sphere.DOScale(Vector3.one, 0.5f);
        _health.canTakeDamage = true;
        DOVirtual.DelayedCall(Random.Range(openDuration.x, openDuration.y), Close);
    }

    void Close()
    {
        sphere.DOScale(Vector3.zero, 0.5f);
        _health.canTakeDamage = false;
        DOVirtual.DelayedCall(Random.Range(openInterval.x, openInterval.y), TryOpen);
    }

    public override void Die(Vector3? hitPoint, Vector3? hitNormal, Vector3? hitImpulse)
    {
        spiderHealth.TakeDamage(damageToHost);
    }
}