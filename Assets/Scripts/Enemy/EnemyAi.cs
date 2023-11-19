using System.Collections;
using System.Collections.Generic;
using EditorCools;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float attackInterval = 3;
    public float attackTimer = 0;

    [CanBeNull]
    private Transform _target;

    private WaspEnemyGun _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _weapon = GetComponent<WaspEnemyGun>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            FindTargetAndShoot();
            attackTimer = attackTimer % attackInterval;
        }

        transform.LookAt(_target);
    }

    [Button()]
    void FindTargetAndShoot()
    {
        if (!_target)
        {
            var player = FindObjectOfType<Player>();
            if (!player)
                return;
            _target = player.GetComponent<Health>().autoAimRoot;
        }

        if (!_target) return;
        _weapon.target = _target;
        _weapon.StartCharging();
    }
}