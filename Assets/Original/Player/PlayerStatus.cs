using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerStatus : MonoBehaviour
{
    public event Action<float> OnHPChanged;
    public event Action<float> OnGasPointChanged;
    public event Action<float> OnToiletPointChanged;

    #region SerializeField
    [SerializeField] 
    private InvincibleSystem _invincibility;
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

    private int _curMoney;

    public int CurrentMoney;

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

    private void OnDisable()
    {
        _hitCollider.OnHitEvent -= OnDamaged;
        CarEnemy.OnRewardDropped -= EarnMoney;
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

    private void OnDamaged(Collider other)
    {
        if (other.CompareTag("Parried"))
        {
            return;
        }

        if (_invincibility.IsActive)
        {
            Debug.Log("Pass Damage");
            return;
        }

        if (other.TryGetComponent<IParryable>(out var parryable))
        {
            _curHealthPoint -= _damage;
            _invincibility.StartInvinble();
            OnHPChanged?.Invoke(_curHealthPoint);
            parryable.OnAttack();
        }
    }

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

    public void RefillGas()
    {
        _curGasPoint = _defaultMaxGasPoint;
        OnGasPointChanged?.Invoke(_curGasPoint);
    }

    public void RefillHP(float amount)
    {
        _curHealthPoint += amount;
        OnHPChanged?.Invoke(_curHealthPoint);
    }

    public void EarnMoney(int amount)
    {
        _curMoney += amount;
    }

    public bool TrySpendMoney(int amount)
    {
        if (CurrentMoney < amount)
        {
            return false;
        }
        else
        {
            _curMoney -= amount;
            return true;
        }
        _curMoney++;
        OnMoneyChanged?.Invoke(_curMoney);
    }

    private void TakeDamage(float amount)
    {
        _curHealthPoint -= _damage;
        OnHPChanged?.Invoke(_curHealthPoint);
    }
}
