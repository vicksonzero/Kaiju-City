using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    
    
    public bool damagedEffectDone;
    public ParticleSystem damagedPs;

    public bool veryDamagedEffectDone;
    public ParticleSystem veryDamagedPs;
    private Health _health;
    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
        if (_health)
        {
            _health.HealthUpdated += OnHealthUpdated;
        }

        damagedPs.Stop();
        veryDamagedPs.Stop();
    }

    
    public void OnHealthUpdated(float hp, float hpMax)
    {
        if (!damagedEffectDone && _health.Percentage <= 0.5f)
        {
            damagedPs.Play();
            damagedEffectDone = true;
        }

        if (!veryDamagedEffectDone && _health.Percentage <= 0.2f)
        {
            veryDamagedPs.Play();
            veryDamagedEffectDone = true;
        }
    }
}
