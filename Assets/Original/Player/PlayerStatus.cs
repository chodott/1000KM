using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public event Action<float> OnHPChanged;
    public event Action<float> OnGasPointChanged;
    public event Action<float> OnToiletPointChanged;

    [SerializeField]
    private float _maxGasPoint;
    [SerializeField]
    private float _maxHealthPoint;
    [SerializeField]
    private float _maxToiletPoint;
    [SerializeField]
    private float _gasUsagePerSec;
    [SerializeField]
    private float _toiletGainPerSec;

    private float _curGasPoint;
    private float _curHealthPoint;
    private float _curToiletPoint;

    private void Start()
    {
        _curGasPoint = _maxGasPoint;
        _curHealthPoint = _maxHealthPoint;
        _curToiletPoint = 0.0f;
    }

    private void Update()
    {
        UseGas(_gasUsagePerSec * Time.deltaTime);

        if(_curToiletPoint < _maxToiletPoint)
        {
            _curToiletPoint += _toiletGainPerSec;
        }
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
