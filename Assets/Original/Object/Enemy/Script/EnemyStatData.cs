using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "Scriptable Objects/EnemyStatData")]
public class EnemyStatData : ScriptableObject
{
    [SerializeField]
    private List<Material> _materials;
    public List<Material> materialVariants { get { return _materials; } }

    [SerializeField]
    private Mesh _mesh;
    public Mesh Mesh { get { return _mesh; } }

    [SerializeField]
    private Quaternion _rotation;
    public Quaternion SpawnRotation { get { return _rotation; } }

    [SerializeField]
    private Vector3 _scale;
    public Vector3 Scale { get { return _scale; } }

    [SerializeField]
    private Vector3 _colliderSize;
    public Vector3 ColliderSize { get { return _colliderSize; } }

    [SerializeField]
    private Vector3 _colliderCenter;
    public Vector3 ColliderCenter { get { return _colliderCenter; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private float _healthPoint;
    public float HealthPoint { get { return _healthPoint; } }

    [SerializeField]
    private float _velocity;
    public float Velocity { get { return _velocity; } }

    [SerializeField]
    private int _reward;
    public int Reward { get { return _reward; } }

}
