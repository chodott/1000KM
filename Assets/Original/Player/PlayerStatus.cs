using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public event Action<float> OnHPChanged;
    public event Action<float> OnGasPointChanged;

    [SerializeField]
    private float _maxGasPoint;
    [SerializeField]
    private float _maxHealthPoint;
    [SerializeField]
    private float _gasUsagePerSec;

    private float _curGasPoint;
    private float _curHealthPoint;

    private void Start()
    {
        _curGasPoint = _maxGasPoint;
        _curHealthPoint = _maxHealthPoint;
    }

    private void Update()
    {
        UseGas(_gasUsagePerSec * Time.deltaTime);
    }

    public void UseGas(float amount)
    {
        _curGasPoint -= amount;
        OnGasPointChanged?.Invoke(_curGasPoint);
    }

    private void OnDamaged(float damage)
    {
        _curHealthPoint -= damage;
        OnHPChanged?.Invoke(_curHealthPoint);
    }
}
