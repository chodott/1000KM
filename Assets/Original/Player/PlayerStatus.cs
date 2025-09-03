using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public event Action<float> OnHPChanged;
    public event Action<float> OnGasPointChanged;
    public event Action<float> OnToiletPointChanged;
    public event Action<bool> OnImmortality;

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
    private float _toiletDamagePerSec;
    [SerializeField]
    private float _gasDamagePerSec;
    #endregion

    Coroutine _gasDamageCoroutine;
    Coroutine _toiletDamageCoroutine;



    private float _gasEfficiency;
    private float _maxHealthPoint;

    private float _curGasPoint;
    private float _curHealthPoint;
    private float _curToiletPoint;

    private int _curMoney;
    private bool _isImmortal = false;
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
        UseGas(Time.deltaTime);
        ChargeToiletGauge(Time.deltaTime);

    }

    private void ChargeToiletGauge(float deltaTime)
    {
        if (_curToiletPoint < _defaultMaxToiletPoint)
        {
            _curToiletPoint += (_toiletGainPerSec * deltaTime);
            OnToiletPointChanged?.Invoke(_curToiletPoint);

            if(_toiletDamageCoroutine != null)
            {
                StopCoroutine(_toiletDamageCoroutine);
                _toiletDamageCoroutine = null;
            }
        }

        else if(_curToiletPoint > _defaultMaxToiletPoint && _toiletDamageCoroutine == null)
        {
            _toiletDamageCoroutine = StartCoroutine(TakeDamageOverToiletGauge());
        }

    }
    #endregion

    public void OnDamaged(float amount)
    {
         _curHealthPoint -= amount;
        if(_curHealthPoint <= 0 && _isImmortal == false)
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

    public void UseGas(float deltaTime)
    {
        if(_curGasPoint <=0 && _gasDamageCoroutine == null)
        {
            _gasDamageCoroutine = StartCoroutine(TakeDamageOverUsedGas());
        }
        else if(_curGasPoint >0)
        {
            _curGasPoint -= _gasEfficiency * deltaTime;
            OnGasPointChanged?.Invoke(_curGasPoint);
            if(_gasDamageCoroutine != null)
            {
                StopCoroutine(_gasDamageCoroutine);
                _gasDamageCoroutine = null;
            }
        }
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

    public void ActivateImmortality()
    {
        _isImmortal =  !_isImmortal;
        OnImmortality?.Invoke(_isImmortal);
    }

    private IEnumerator TakeDamageOverUsedGas()
    {
        while (true)
        {
            OnDamaged(_gasDamagePerSec);
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator TakeDamageOverToiletGauge()
    {
        while (true)
        {
            OnDamaged(_toiletDamagePerSec); 
            yield return new WaitForSeconds(1f);
        }
    }
}
