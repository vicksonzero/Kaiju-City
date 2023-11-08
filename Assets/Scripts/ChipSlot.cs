using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSlot : MonoBehaviour
{
    public string displayName;
    public string description;
    public UpgradeType upgradeType;
    public ChipSO chip;

    public ChipSO RemoveChip()
    {
        var oldChip = chip;
        // TODO: do remove-chip callbacks

        return oldChip;
    }
    public void AddChip(ChipSO chip)
    {
        this.chip = chip;

        // TODO: do assign-chip callbacks
    }
}