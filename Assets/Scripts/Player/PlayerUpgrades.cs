using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChips : MonoBehaviour
{
    public ChipSlot[] chipSlots;

    public Dictionary<UpgradeType, int> upgrades = new Dictionary<UpgradeType, int>();
    // Start is called before the first frame update
    void Start()
    {
        chipSlots = GetComponents<ChipSlot>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int GetUpgradeByType(UpgradeType upgradeType)
    {
        upgrades.TryGetValue(upgradeType, out var stacks);
        return stacks;
    }

    public bool HasUpgrade(UpgradeType upgradeType)
    {
        return upgrades.TryGetValue(upgradeType, out var stacks) && stacks > 0;
    }

    public void SwapChips(int fromId, int toId)
    {
        var chipFrom = chipSlots[fromId].RemoveChip();
        var chipTo = chipSlots[toId].RemoveChip();

        chipSlots[fromId].AddChip(chipTo);
        chipSlots[toId].AddChip(chipFrom);
    }
}