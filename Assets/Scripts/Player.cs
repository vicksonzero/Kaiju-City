using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hi");
        BuildNumberSO.GetAsset(so => Debug.Log($"build number: {(so ? so.buildNumber : "")}"));
    }

    // Update is called once per frame
    void Update()
    {
    }
}