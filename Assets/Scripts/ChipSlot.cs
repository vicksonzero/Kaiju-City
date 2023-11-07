using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSlot : MonoBehaviour
{
    public string displayName;
    public string description;
    public UpgradeType upgradeType;
    public Chip chip;

    public Chip RemoveChip()
    {
        var oldChip = chip;
        // TODO: do remove-chip callbacks

        return oldChip;
    }
    public void AddChip(Chip chip)
    {
        this.chip = chip;

        // TODO: do assign-chip callbacks
    }
}