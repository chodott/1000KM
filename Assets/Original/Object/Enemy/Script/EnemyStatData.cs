using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "Scriptable Objects/EnemyStatData")]
public class EnemyStatData : ScriptableObject
{
    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private float _healthPoint;
    public float HealthPoint { get { return _healthPoint; } }

    [SerializeField]
    private float _velocity;
    public float Velocity { get { return _velocity; } }

}
