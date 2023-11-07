using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreen : MonoBehaviour
{
    public PlayerChips playerChips;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hi");
        // BuildNumberSO.GetAsset(so => Debug.Log($"build number: {(so ? so.buildNumber : "")}"));
    }

    public void SwapChips(int fromId, int toId)
    {
        if (!playerChips) return;
        playerChips.SwapChips(fromId, toId);
    }
}