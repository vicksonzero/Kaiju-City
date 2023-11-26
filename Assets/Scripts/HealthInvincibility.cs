using System;
using DG.Tweening;
using EditorCools;
using JetBrains.Annotations;
using StarterAssets;
using UnityEngine;

public class HealthInvincibility : MonoBehaviour
{
    public bool applyInvincibilityOnDamageTaken = true;
    public float invincibilityTime = 3;
    private Health _health;

    private int _animIdIsHurt;
    private float _invincibleUntil = -1;

    private void Awake()
    {
        _health = GetComponent<Health>();
        if (_health)
        {
            _health.DamageTaken += OnDamageTaken;
        }
    }

    private void Update()
    {
        if (_invincibleUntil > 0 && Time.time > _invincibleUntil)
        {
            _health.canTakeDamage = true;
            _invincibleUntil = -1;
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
        Debug.Log($"ApplyTimedInvincibility {duration}");
        _health.canTakeDamage = false;
        _invincibleUntil = Math.Max(_invincibleUntil, Time.time + duration);
    }
}