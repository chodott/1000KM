using UnityEngine;

public class BossEnemyController : MonoBehaviour, IDamagable
{
    #region SerializeField
    [SerializeField]
    private BossPhaseData[] _phaseDatas;
    [SerializeField]
    private GameObject[] _projectilePrefabs;
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
    #endregion 


    private StateMachine<BossEnemyController> _stateMachine;
    private DropCargoState _dropCargoState = new DropCargoState();
    private BossStunState _stunState = new BossStunState();

    private Vector3 _forwardVector = -Vector3.right;
    private float _curVelocity;

    private float _curHealthPoint;
    private int _curPhaseIndex;

    public LaneMover LaneMover { get { return _laneMover; } }

    private void Awake()
    {
        _stateMachine = new StateMachine<BossEnemyController>(this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_dropCargoState);
        UpdatePhase();
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    private void UpdatePhase()
    {
        if(_curPhaseIndex + 1 < _phaseDatas.Length)
        {
            var nextPhase = _phaseDatas[_curPhaseIndex + 1];
            if(nextPhase.HpThreshold < _curHealthPoint)
            {
                return;
            }
            _curPhaseIndex++;
            _laneMover.UpdateMoveLaneSpeed(nextPhase.MoveSpeed);
        }
    }

    public void DropProjectile()
    {
        var phase = _phaseDatas[_curPhaseIndex];
        int randomIndex = UnityEngine.Random.Range(phase.UseProjectileRanage.x, phase.UseProjectileRanage.y+1);
        GameObject spawnPrefab = _projectilePrefabs[randomIndex];
        Instantiate(spawnPrefab, transform.position, transform.rotation);
    }

    public void ChangeDropState()
    {
        _stateMachine.ChangeState(_dropCargoState);
    }

    public void ChangeStunState()
    {
        _stateMachine.ChangeState(_stunState);
    }

    public void OnDamaged(float amount)
    {
        _curHealthPoint -= amount;
        UpdatePhase();
        if (_curHealthPoint < 0)
        {
            
        }
    }
}
