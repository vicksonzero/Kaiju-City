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
        EntityCounter.Inst.Add(channels, this);
    }

    private void OnDestroy()
    {
        EntityCounter.Inst.Remove(channels, this);
    }
}
