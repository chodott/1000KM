using UnityEngine;

[CreateAssetMenu(fileName = "PartStatus", menuName = "Scriptable Objects/PartStatus")]
public class PartStatus : ScriptableObject
{
    [SerializeField]
    private PartType _partType;
    public PartType Type { get { return _partType; } }
    [SerializeField]
    private float _accelerationBonus;
    public float AccelerationBonus { get { return _accelerationBonus; } }

    [SerializeField]
    private float _maxHpBonus;
    public float MaxHpBonus { get { return _maxHpBonus; } }

    [SerializeField]
    private float _laneMoveSpeedBonus;
    public float LaneMoveSpeedBonus { get {return _laneMoveSpeedBonus; } }

    [SerializeField]
    private float _gasEfficiencyBonus;
    public float GasEfficiencyBonus { get { return _gasEfficiencyBonus; } }

    [SerializeField]
    private float _dragReduction;
    public float DragReduction { get { return _dragReduction; } }

    public void AddStatus(PartStatus otherStatus)
    {
        _accelerationBonus += otherStatus.AccelerationBonus;
        _maxHpBonus += otherStatus.MaxHpBonus;
        _laneMoveSpeedBonus += otherStatus.LaneMoveSpeedBonus;
        _gasEfficiencyBonus += otherStatus.GasEfficiencyBonus;
        _dragReduction += otherStatus.DragReduction;   
    }
}
