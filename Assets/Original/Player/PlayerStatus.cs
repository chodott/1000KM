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
    #endregion

    private float _gasEfficiency;
    private float _maxHealthPoint;

    private float _curGasPoint;
    private float _curHealthPoint;
    private float _curToiletPoint;

    private int _curMoney;
    public int CurrentMoney { get { return _curMoney; } }

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
        CarEnemy.OnRewardDropped += EarnMoney;
    }

    private void OnDisable()
    {
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

    public void OnDamaged(float amount)
    {
         _curHealthPoint -= amount;
        if(_curHealthPoint <= 0)
        {
            GameEvents.SetPhase(GamePhase.GameOver);
        }
         OnHPChanged?.Invoke(_curHealthPoint);
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

    public void RefillHP()
    {
        _curHealthPoint = _maxHealthPoint;
        OnHPChanged?.Invoke(_curHealthPoint);
    }

    public void RefillToilet()
    {
        _curToiletPoint = 0;
        OnToiletPointChanged?.Invoke(_curToiletPoint);
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
    }
}
