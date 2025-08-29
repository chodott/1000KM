using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState : IState<BossEnemyController>
{

}

public class PaceToDistance : IBossState
{
    BossEnemyController _bossEnemy;
    private float _keepDistance;

    public PaceToDistance(float distance)
    {
        _keepDistance = distance;
    }

    public void Enter(BossEnemyController owner)
    {
        _bossEnemy = owner;
    }

    public void Exit()
    {
        _bossEnemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        return;
    }

    public void Update()
    {
        _bossEnemy.MoveToBack();
        if(_bossEnemy.GetDistanceToPlayer() <= _keepDistance)
        {
            _bossEnemy.ChangeDropState();
        }
        return;
    }
}


public class DropCargoState : IBossState
{
    private BossEnemyController _bossEnemy;
    private LaneMover _laneMover;

    private event Action _onCycleFinished;
    private (int, int) _moveCountRange = (3, 6);
    private int _leftMoveCount;
    public void Enter(BossEnemyController bossEnemyController)
    {
        _bossEnemy = bossEnemyController;
        _laneMover = _bossEnemy.LaneMover;

        _laneMover.OnFinishMove += ArriveLane;
        DoCycle();
    }

    public void Exit()
    {
        _laneMover.OnFinishMove -= ArriveLane;
        _laneMover = null;
        _bossEnemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        return;
    }

    public void Update()
    {
    }

    public void DoCycle()
    {
        _leftMoveCount = UnityEngine.Random.Range(_moveCountRange.Item1, _moveCountRange.Item2);
        MoveLane();
    }

    public void ArriveLane()
    {
        _leftMoveCount--;
        if (_leftMoveCount == 0)
        {
            DropCargo();
            _bossEnemy.ChangeStunState();
        }
        else
        {
            MoveLane();
        }
    }

    public void MoveLane()
    {
        float direction = UnityEngine.Random.Range(0, 2) * 2 - 1;
        bool result = _laneMover.MoveLane(direction);
        if(result == false)
        {
            _laneMover.MoveLane(-direction);
        }
    }

    public void DropCargo()
    {
        _bossEnemy.DropProjectile();
    }
}


public class BossStunState : IBossState
{
    BossEnemyController _bossEnemy;
    private Coroutine _coroutineHandle;
    private float _stunTime = 2f;
    public void Enter(BossEnemyController owner)
    {
        _bossEnemy = owner;
        _coroutineHandle = _bossEnemy.StartCoroutine(Stun());
    }

    public void Exit()
    {
        _bossEnemy.StopCoroutine(_coroutineHandle);
        _bossEnemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
    }

    public void Update()
    {
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(_stunTime);

        _bossEnemy.ChangeDropState();
        _coroutineHandle = null;
    }
}
