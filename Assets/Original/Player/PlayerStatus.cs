using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public event Action<float> OnHPChanged;
    public event Action<float> OnGasPointChanged;
    public event Action<float> OnToiletPointChanged;
    public event Action<int> OnMoneyChanged;

    #region SerializeField
    [SerializeField]
    private HitCollider _hitCollider;
    [SerializeField]
    private float _defaultMaxGasPoint;
    [SerializeField]
    private float _defaultMaxHealthPoint;
    [SerializeField]
    private float _defaultMaxToiletPoint;
    [SerializeField]
    private float _defaultGasEfficiency;
    [SerializeField]
    private float _toiletGainPerSec;
    [SerializeField]
    private float _damage = 10.0f;
    #endregion

    private float _gasEfficiency;
    private float _maxHealthPoint;

    private float _curGasPoint;
    private float _curHealthPoint;
    private float _curToiletPoint;
    private int _curMoney = 0;

    #region Monobehaviour Callbacks
    private void Start()
    {
        _curGasPoint = _defaultMaxGasPoint;
        _curHealthPoint = _defaultMaxHealthPoint;
        _curToiletPoint = 0.0f;
        OnHPChanged?.Invoke(_curHealthPoint);
    }

    private void OnEnable()
    {
        _hitCollider.OnHitEvent += TakeDamage;
        CarEnemy.OnRewardDropped += EarnMoney;
    }

    private void Update()
    {
        UseGas(_gasEfficiency * Time.deltaTime);

        if(_curToiletPoint < _defaultMaxToiletPoint)
        {
            _curToiletPoint += (_toiletGainPerSec * Time.deltaTime);
            OnToiletPointChanged?.Invoke(_curToiletPoint);
        }
    }
    #endregion

    public void UpdateStatus(PartStatus status)
    {
        _gasEfficiency = _defaultGasEfficiency - status.GasEfficiencyBonus;
        _maxHealthPoint = status.MaxHpBonus + _defaultMaxHealthPoint;
        _curHealthPoint = _maxHealthPoint;
        OnHPChanged?.Invoke(_curHealthPoint);

    }

    public void UseGas(float amount)
    {
        _curGasPoint -= amount;
        OnGasPointChanged?.Invoke(_curGasPoint);
    }

    private void EarnMoney()
    {
        _curMoney++;
        OnMoneyChanged?.Invoke(_curMoney);
    }

    private void TakeDamage(float amount)
    {
        _curHealthPoint -= _damage;
        OnHPChanged?.Invoke(_curHealthPoint);
    }
}
