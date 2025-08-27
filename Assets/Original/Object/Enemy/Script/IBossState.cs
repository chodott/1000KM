using System;
using UnityEngine;

public interface IBossState : IState<BossEnemyController>
{

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

        _laneMover.OnFinishMove += MoveLane;
        DoCycle();
    }

    public void Exit()
    {
        _laneMover = null;
        _bossEnemy = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
    }

    public void DoCycle()
    {
        _leftMoveCount = UnityEngine.Random.Range(_moveCountRange.Item1, _moveCountRange.Item2);
        MoveLane();
    }
    public void MoveLane()
    {
        if(_leftMoveCount==0)
        {
            DropCargo();
        }
        else
        {
            _leftMoveCount--;
            float direction = UnityEngine.Random.Range(0, 2) * 2 - 1;
            bool result = _laneMover.MoveLane(direction);
            if(result == false)
            {
                _laneMover.MoveLane(-direction);
            }
        }
    }

    public void DropCargo()
    {
        DoCycle();
    }
}
