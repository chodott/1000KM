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
    protected Transform _targetTransform;

    protected int _curPhaseIndex = -1;

    private void OnEnable()
    {
        _curHealthPoint = _maxHealthPoint;
    }

    private void Start()
    {
        UpdatePhase();
    }

    private void Update()
    {
        _curVelocity = GlobalMovementController.Instance.GlobalVelocity;
    }

    public virtual void UpdatePhase()
    {
    }

    public void Init(Transform playerTransform)
    {
        _targetTransform = playerTransform;
    }

    public float GetXDistanceToPlayer()
    {
        return Mathf.Abs(Rb.position.x);
    }

    public float GetZOffsetToPlayer()
    {
        float zOffset =  _targetTransform.position.z- Rb.position.z;
        return zOffset;
    }

    public void MoveToForward(float direction)
    {
        Vector3 nextPosition = Rb.position + direction * Vector3.right * _velocity * Time.fixedDeltaTime;
        Rb.MovePosition(nextPosition);
    }

    public void KnockbackToSide(float direction)
    {
        bool canMove = LaneMover.KnockbackLane(direction);
        if(canMove == false)
        {
            LaneMover.PlayKnockbackAnim(direction);
        }
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

    public bool OnDamaged(float amount)
    {
        _curHealthPoint -= amount;
        if (_curHealthPoint < 0)
        {
            GameEvents.RaiseBossDefeated();
        }
        else
        {
            UpdatePhase();
        }
        return true;

    }
}
