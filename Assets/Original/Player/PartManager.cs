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
    private Dictionary<PartType, PartStatus> _equippedPartMap = new Dictionary<PartType, PartStatus>();

    public event Action<PartStatus> OnChangedPartStatus;

    public void EquipNewPart(PartStatus newPart)
    {
        var type = newPart.Type;
        _equippedPartMap[type] = newPart;
        OnChangedPartStatus?.Invoke(CalculateStats());
    }

    public PartStatus CalculateStats()
    {
        var baseStatus = new PartStatus();
        foreach(PartStatus partStatus in _equippedPartMap.Values)
        {
            baseStatus.AddStatus(partStatus);
        }

        return baseStatus;
    }

}
