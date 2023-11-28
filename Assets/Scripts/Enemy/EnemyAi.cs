using System.Collections;
using System.Collections.Generic;
using EditorCools;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public Vector2 attackInterval = new Vector2(3, 5);
    private float _attackInterval = 3;
    public float attackTimer = 0;

    [CanBeNull]
    private Transform _target;

    private WaspEnemyGun _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _weapon = GetComponent<WaspEnemyGun>();
        _attackInterval = Random.Range(attackInterval.x, attackInterval.y);
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= _attackInterval)
        {
            FindTargetAndShoot();
            attackTimer = 0;
            _attackInterval = Random.Range(attackInterval.x, attackInterval.y);
        }

        transform.LookAt(_target);
    }

    [Button()]
    void FindTargetAndShoot()
    {
        if (!_target || !_target.gameObject.activeInHierarchy)
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