using System;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public class SpiderDamageTakenHandler : MonoBehaviour
{
    public float punchTime = 2;
    public float punchMultiplier = 1;
    public Transform displaceRoot;
    public PersistingGizmoLine debugImpulseLinePrefab;

    private Tween _displaceTween;

    private void Start()
    {
        var health = GetComponent<Health>();
        if (health)
        {
            health.DamageTaken += OnDamageTaken;
        }
    }

    private void OnDamageTaken(float hp, float max, Vector3? point, Vector3? normal, Vector3? impulse)
    {
        Debug.Log($"{nameof(SpiderDamageTakenHandler)}.{nameof(OnDamageTaken)}: {impulse?.magnitude}");
        if (impulse == null || impulse.Value.magnitude < 1) return;
        
        var relativeImpulse = displaceRoot.InverseTransformDirection(impulse.Value);

        // var debugImpulseLine = Instantiate(debugImpulseLinePrefab, point ?? transform.position, Quaternion.identity);
        // debugImpulseLine.direction = impulse.Value;

        _displaceTween?.Kill();
        _displaceTween = displaceRoot.DOPunchPosition(relativeImpulse * punchMultiplier, punchTime, 5, 0.5f)
            .SetUpdate(UpdateType.Late);
    }
}