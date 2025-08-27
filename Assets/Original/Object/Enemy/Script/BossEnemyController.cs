using UnityEngine;

public class BossEnemyController : MonoBehaviour
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
    #endregion 


    private StateMachine<BossEnemyController> _stateMachine;
    private DropCargoState _dropCargoState = new DropCargoState();
    private Vector3 _forwardVector = -Vector3.right;
    private float _curVelocity;

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
        MoveForward(_curVelocity);
        _stateMachine.Update();
    }

    private void MoveForward(float velocity)
    {
        _curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        Vector3 targetPosition = _rigidbody.position + (_forwardVector * _curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    public void ChangeState(IBossState state)
    {
        _stateMachine.ChangeState(state);
    }

    public void DropProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }

}
