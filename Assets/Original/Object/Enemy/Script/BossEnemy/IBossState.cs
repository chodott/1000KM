using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MatchGapState : IState<BossEnemyController>
{
    BossEnemyController _controller;
    private float _keepDistance;
    private float _direction;

    public MatchGapState(float distance)
    {
        _keepDistance = distance;
    }

    public void Enter(BossEnemyController owner)
    {
        _controller = owner;
        float distance = _controller.GetXDistanceToPlayer();
        _direction = distance < _keepDistance ? -1 : 1;
    }

    public void Exit()
    {
        _controller = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
    }

    public void OnTriggerEnter(Collider other)
    {
    }


    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        return;
    }

    public void Update()
    {
        _controller.MoveToForward(_direction);
        float distance = Mathf.Abs(_controller.GetXDistanceToPlayer() - _keepDistance);
        if(distance <= 1f)
        {
            _controller.OnMatchGapEnd();
        }
    }
}

public class MoveShuffleState : IState<BossEnemyController>
{
    private BossEnemyController _controller;
    private LaneMover _laneMover;
    private (int, int) _moveCountRange = (3, 6);
    private int _leftMoveCount;
    public void Enter(BossEnemyController owner)
    {
        _controller = owner;
        _laneMover = _controller.LaneMover;

        _laneMover.OnFinishMove += ArriveLane;
        DoCycle();
    }

    public void Exit()
    {
        _laneMover.OnFinishMove -= ArriveLane;
        _controller = null;
        _laneMover = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
    }

    public void OnTriggerEnter(Collider other)
    {
    }


    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
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
            _controller.OnMoveShuffleEnd();
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
        if (result == false)
        {
            _laneMover.MoveLane(-direction);
        }
    }
}

public class BossStunState : IState<BossEnemyController>
{
    BossEnemyController _controller;
    private Coroutine _coroutineHandle;
    private float _stunTime = 2f;
    public void Enter(BossEnemyController owner)
    {
        _controller = owner;
        _coroutineHandle = _controller.StartCoroutine(Stun());
    }

    public void Exit()
    {
        if(_coroutineHandle != null)
        {
            _controller.StopCoroutine(_coroutineHandle);
        }
        _controller = null;
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
    public void OnTriggerEnter(Collider other)
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
        _coroutineHandle = null;

        _controller.OnStunEnd();
    }
}


public class BossDestroyState : IState<BossEnemyController>
{
    BossEnemyController _controller;
    public void Enter(BossEnemyController owner)
    {
        _controller = owner;
        _controller.BurstSystem.Play();
    }

    public void Exit()
    {
        GameEvents.RaiseBossDefeated();
        _controller = null;
    }

    public void OnCollisionEnter(Collision collision)
    {

    }
    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
    }

    public void Update()
    {
        _controller.MoveToForward(1);
        if (_controller.Rb.position.x >= 5f)
        {
            _controller.OnDestroyEnd();
        }
    }
}
