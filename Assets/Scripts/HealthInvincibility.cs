using System;
using DG.Tweening;
using EditorCools;
using StarterAssets;
using UnityEngine;

public class HealthInvincibility : MonoBehaviour
{
    public bool applyInvincibilityOnDamageTaken = true;
    public float invincibilityTime = 3;
    private Health _health;

    private int _animIdIsHurt;

    private void Start()
    {
        _health = GetComponent<Health>();
        if (_health)
        {
            _health.DamageTaken += OnDamageTaken;
        }
    }

    private void OnDamageTaken(float hp, float max, Vector3? point, Vector3? normal, Vector3? impulse)
    {
        if (applyInvincibilityOnDamageTaken)
            ApplyTimedInvincibility(invincibilityTime);
    }

    [Button()]
    public void ApplyTimedInvincibility() => ApplyTimedInvincibility(invincibilityTime);

    public void ApplyTimedInvincibility(float duration)
    {
        _health.canTakeDamage = false;
        DOVirtual.DelayedCall(duration, () => _health.canTakeDamage = true);
    }
}