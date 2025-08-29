using UnityEngine;

public class BossEnemyController : BaseEnemy, IDamagable
{
    #region SerializeField
    [SerializeField]
    protected BossPhaseData[] _phaseDatas;
    [SerializeField]
    private GameObject _destroyEffectPrefab;
    [SerializeField]
    private float _maxHealthPoint = 6;
    [SerializeField]
    private float _velocity = 5f;
    #endregion 


    protected MoveShuffleState _moveShuffleState = new MoveShuffleState();
    protected BossStunState _stunState = new BossStunState();

    protected int _curPhaseIndex = -1;

    private void OnEnable()
    {
        _curHealthPoint = _maxHealthPoint;
    }

    private void Start()
    {
        UpdatePhase();
    }

    public virtual void UpdatePhase()
    {
    }

    public float GetXDistanceToPlayer()
    {
        return Mathf.Abs(Rb.position.x);
    }

    public float GetZOffsetToPlayer()
    {
        return Rb.position.z;
    }

    public void MoveToBack()
    {
        Vector3 nextPosition = Rb.position + Vector3.right * _velocity * Time.fixedDeltaTime;
        Rb.MovePosition(nextPosition);
    }

    public virtual void ChangeStunState()
    {
    }

    public virtual void ChangeShuffleState()
    {
    }

    public virtual void OnMatchGapEnd()
    {

    }

    public virtual void OnMoveShuffleEnd()
    {

    }


    public virtual void OnStunEnd()
    {

    }

    public void OnDamaged(float amount)
    {
        _curHealthPoint -= amount;
        if (_curHealthPoint < 0)
        {

        }
        else
        {
            UpdatePhase();

        }
    }
}
