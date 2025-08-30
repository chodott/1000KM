
using UnityEngine;

public class CargoTruckController : BossEnemyController
{
    [SerializeField]
    private GameObject[] _projectilePrefabs;

    private StateMachine<CargoTruckController> _stateMachine = new StateMachine<CargoTruckController>();
    private DropCargoState _dropCargoState = new DropCargoState();

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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

    public void ChangeDropState()
    {
        _stateMachine.ChangeState(_dropCargoState, this);
    }

    public override void OnMoveShuffleEnd()
    {
        ChangeDropState();
    }

    public override void OnStunEnd()
    {
        ChangeShuffleState();
    }

    public override void OnMatchGapEnd()
    {
        ChangeShuffleState();
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