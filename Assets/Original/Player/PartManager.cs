using System;
using System.Collections.Generic;
using UnityEngine;

public class PartManager : MonoBehaviour
{
    //Temp Debug Code
    private void Start()
    {
        OnChangedPartStatus?.Invoke(CalculateStats());
    }

    [SerializeField]
    List<PartStatus> _equippedParts = new List<PartStatus>();

    public event Action<PartStatus> OnChangedPartStatus;

    public void EquipNewPart(PartStatus newPart)
    {
        var type = newPart.Type;
        _equippedParts[(int)type] = newPart;
        OnChangedPartStatus?.Invoke(CalculateStats());
    }

    public PartStatus CalculateStats()
    {
        var baseStatus = new PartStatus();
        foreach(PartStatus partStatus in _equippedParts)
        {
            baseStatus.AddStatus(partStatus);
        }

        return baseStatus;
    }

}
