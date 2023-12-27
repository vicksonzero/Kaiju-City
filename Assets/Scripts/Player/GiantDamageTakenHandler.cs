using System;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public class GiantDamageTakenHandler : MonoBehaviour
{
    public float stunTime = 2;
    private Animator _animator;
    private ThirdPersonController _controller;

    private int _animIdIsHurt;

    private void Start()
    {
        _controller = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();

        _animIdIsHurt = Animator.StringToHash("IsHurt");
        var health = GetComponent<Health>();
        if (health)
        {
            health.DamageTaken += OnDamageTaken;
        }
    }

    private void OnDamageTaken(float hp, float max, Vector3? point, Vector3? normal, Vector3? impulse)
    {
        Debug.Log($"{nameof(OnDamageTaken)}, {impulse?.magnitude}");
        if (impulse?.magnitude < 1) return;

        _controller.canControlMovement = false;
        _animator.SetBool(_animIdIsHurt, true);
        DOVirtual.DelayedCall(stunTime, () =>
        {
            _controller.canControlMovement = true;
            _animator.SetBool(_animIdIsHurt, false);
        }, false);
        GetComponent<HealthInvincibility>().ApplyTimedInvincibility();
    }
}