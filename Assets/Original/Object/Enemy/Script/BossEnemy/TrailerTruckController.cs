
using UnityEngine;

public class TrailerTruckController : BossEnemyController
{
    [SerializeField]
    private GameObject[] _projectilePrefabs;

    private StateMachine<TrailerTruckController> _stateMachine;
    private DropCargoState _dropCargoState = new DropCargoState();

    private void Awake()
    {
        _stateMachine = new StateMachine<TrailerTruckController>();
    }

    protected override void FixedUpdate()
    {
        _stateMachine.Update();
    }
    public void DropProjectile()
    {
        var phase = _phaseDatas[_curPhaseIndex];
        int randomIndex = UnityEngine.Random.Range(phase.UseProjectileRanage.x, phase.UseProjectileRanage.y + 1);
        GameObject spawnPrefab = _projectilePrefabs[randomIndex];
        Instantiate(spawnPrefab, transform.position, transform.rotation);
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
       
    }

    public override void OnStunEnd()
    {
        ChangeShuffleState();
    }

    public override void OnMatchGapEnd()
    {
        ChangeShuffleState();
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

}