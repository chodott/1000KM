
using UnityEngine;

public class TrailerTruckController : BossEnemyController, IParryable
{
    [SerializeField]
    private GameObject[] _projectilePrefabs;

    private StateMachine<TrailerTruckController> _stateMachine;
    private ChaseState _chaseState = new ChaseState();
    private PressState _pressState = new PressState();
    private float GapToPlayer = 15f;

    private void Awake()
    {
        _stateMachine = new StateMachine<TrailerTruckController>();
    }

    private void Start()
    {
        _stateMachine.ChangeState(new MatchGapState(15f), this);
    }

    protected override void FixedUpdate()
    {
        _stateMachine.Update();
    }

    public void RecoverVelocity()
    {
        if (_curVelocity <= GlobalMovementController.Instance.GlobalVelocity)
        {
            _stateMachine.ChangeState(new MatchGapState(GapToPlayer), this);
        }
    }

    public override void UpdatePhase()
    {
        if (_curPhaseIndex + 1 < _phaseDatas.Length)
        {
            var nextPhase = _phaseDatas[_curPhaseIndex + 1];
            if (nextPhase.HpThreshold < _curHealthPoint)
            {
                return;
            }
            _curPhaseIndex++;
            LaneMover.UpdateMoveLaneSpeed(nextPhase.MoveSpeed);

            float mustKeepDistance = nextPhase.StandOffDistance;
            if (Mathf.Abs(Rb.position.x) > mustKeepDistance)
            {
                _stateMachine.ChangeState(new MatchGapState(mustKeepDistance), this);
            }
        }
    }

    public override void OnMoveShuffleEnd()
    {
        _stateMachine.ChangeState(_pressState, this);
    }

    public override void OnStunEnd()
    {
        ChangeShuffleState();
    }

    public override void OnMatchGapEnd()
    {
        ChangeShuffleState();
    }

    public void OnPressEnd()
    {
        _stateMachine.ChangeState(_chaseState, this);
    }

    public void OnChaseEnd()
    {
        _stateMachine.ChangeState(new MatchGapState(GapToPlayer), this);
    }

    public void ChangeMatchState()
    {

    }

    public override void ChangeStunState()
    {
        _stateMachine.ChangeState(_stunState, this);
    }

    public override void ChangeShuffleState()
    {
        _stateMachine.ChangeState(_moveShuffleState, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        _stateMachine.OnTriggerEnter(other);
    }

    public void OnParried(Vector3 contactPosition, float damage, float moveLaneSpeed)
    {
        _stateMachine.OnParried(contactPosition, damage, moveLaneSpeed);
    }

    public void OnAttack()
    {
        throw new System.NotImplementedException();
    }
}