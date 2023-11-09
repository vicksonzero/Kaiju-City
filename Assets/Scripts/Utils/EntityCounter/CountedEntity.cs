using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountedEntity : MonoBehaviour
{
    public string[] channels;

    // Start is called before the first frame update
    void Start()
    {
        if (EntityCounter.Inst) EntityCounter.Inst.Add(channels, this);
    }

    private void OnDestroy()
    {
        if (EntityCounter.Inst) EntityCounter.Inst.Remove(channels, this);
    }
}