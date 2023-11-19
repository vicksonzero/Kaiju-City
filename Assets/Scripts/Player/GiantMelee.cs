using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class GiantMelee : MonoBehaviour
{
    public float kineticDamage = 10;
    public Bullet bulletPrefab;
    public float kickImpulse = 1f;

    public Transform hitEffectPrefab;
    public Transform bulletDisplayList;
    public Transform effectDisplayList;

    public Transform leftPunchRoot;
    public Transform rightPunchRoot;
    private StarterAssetsInputs _inputs;
    private Animator _animator;

    // animation IDs
    private int _animIDPunch1;

    // Start is called before the first frame update
    void Start()
    {
        _inputs = FindObjectOfType<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();

        _animIDPunch1 = Animator.StringToHash("Punch1");
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.shoot)
        {
            _animator.SetBool(_animIDPunch1, true);
        }
        else
        {
            _animator.SetBool(_animIDPunch1, false);
        }
    }

    void DoBullet(int isLeft)
    {
        Debug.Log($"{nameof(DoBullet)} {isLeft}");

        var root = isLeft == 1 ? leftPunchRoot : rightPunchRoot;
        Instantiate(hitEffectPrefab, root.position, root.rotation, effectDisplayList);
        var bullet = Instantiate(bulletPrefab, root.position, root.rotation, bulletDisplayList);

        bullet.kineticDamage = kineticDamage;
        bullet.effectDisplayList = effectDisplayList;
        bullet.kickImpulse = kickImpulse;
    }
}