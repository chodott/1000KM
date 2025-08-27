using UnityEngine;

public class BossEnemyController : MonoBehaviour, IDamagable
{
    #region SerializeField
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private BoxCollider _collider;
    [SerializeField]
    private GameObject _destroyEffectPrefab;
    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private float _maxHealthPoint = 6;
    #endregion 


    private StateMachine<BossEnemyController> _stateMachine;
    private DropCargoState _dropCargoState = new DropCargoState();
    private Vector3 _forwardVector = -Vector3.right;
    private float _curVelocity;

    private float _curHealthPoint;

    public LaneMover LaneMover { get { return _laneMover; } }

    private void Awake()
    {
        _stateMachine = new StateMachine<BossEnemyController>(this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_dropCargoState);
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    public void ChangeState(IBossState state)
    {
        _stateMachine.ChangeState(state);
    }

    public void DropProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }

    public void OnDamaged(float amount)
    {
        _curHealthPoint -= amount;
        Debug.Log(_curHealthPoint);
        if (_curHealthPoint < 0)
        {
            
        }
    }
}
