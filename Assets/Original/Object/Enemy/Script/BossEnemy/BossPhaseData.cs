using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhaseSO", menuName = "Scriptable Objects/BossPhaseSO")]
public class BossPhaseData : ScriptableObject
{
    [SerializeField, Min(1)]
    private int _hpThreshold;
    public int HpThreshold { get { return _hpThreshold; } }

    [SerializeField, Min(1f)]
    private float _moveSpeed = 8f;
    public float MoveSpeed { get { return _moveSpeed;} }

    [SerializeField]
    private Vector2Int _useProjectileRange = new Vector2Int();
    public Vector2Int UseProjectileRanage { get { return _useProjectileRange; } }
}
