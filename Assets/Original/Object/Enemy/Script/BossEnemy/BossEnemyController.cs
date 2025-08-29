using UnityEngine;

public class BossEnemyController : MonoBehaviour, IDamagable
{
    #region SerializeField
    [SerializeField]
    protected BossPhaseData[] _phaseDatas;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private BoxCollider _collider;
    [SerializeField]
    private GameObject _destroyEffectPrefab;
    [SerializeField]
    private float _maxHealthPoint = 6;
    [SerializeField]
    private float _velocity = 5f;
    #endregion 


    protected MoveShuffleState _moveShuffleState = new MoveShuffleState();
    protected BossStunState _stunState = new BossStunState();

    protected float _curHealthPoint;
    protected int _curPhaseIndex = -1;

    public LaneMover LaneMover { get { return _laneMover; } }
    public Rigidbody Rb { get { return _rigidbody; } }


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

    public float GetDistanceToPlayer()
    {
        return Mathf.Abs(_rigidbody.position.x);
    }

    public void MoveToBack()
    {
        Vector3 nextPosition = _rigidbody.position + Vector3.right * _velocity * Time.fixedDeltaTime;
        _rigidbody.MovePosition(nextPosition);
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
