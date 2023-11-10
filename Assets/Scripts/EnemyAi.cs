using System.Collections;
using System.Collections.Generic;
using EditorCools;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float attackInterval = 3;
    public float attackTimer = 0;

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
        _target = _target ? _target : FindObjectOfType<Player>().transform;
        _weapon.target = _target;
        _weapon.StartCharging();
    }
}