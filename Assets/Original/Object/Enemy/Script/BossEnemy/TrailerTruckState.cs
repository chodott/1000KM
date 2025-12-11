
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PressState : IState<TrailerTruckController>
{
    TrailerTruckController _controller;
    private float _keepDistance = 5f;
    public void Enter(TrailerTruckController owner)
    {
        _controller = owner;
    }

    public void Exit()
    {
        _controller = null;
    }

    public void Update()
    {
        _controller.MoveToForward(1);
        if (_controller.GetXDistanceToPlayer() <= _keepDistance)
        {
            _controller.OnPressEnd();
        }
    }

    public void HandleEvent(StateEvent stateEvent)
    {
        switch (stateEvent)
        {
            case OnTriggerEnterEvent:
                _controller.ChangeMatchState();
                break;
        }
    }
}

public class ChaseState : IState<TrailerTruckController>
{
    private TrailerTruckController _controller;
    private float _direction;
    private bool _isHit = false;
    public void Enter(TrailerTruckController owner)
    {
        _controller = owner;
        float offset = owner.GetZOffsetToPlayer();
        _direction = (int)Mathf.Sign(offset);
        _isHit = false;

        owner.LaneMover.MoveLane(_direction);
        owner.LaneMover.OnFinishMove += ArriveLane;
    }

    public void Exit()
    {
        _controller.LaneMover.OnFinishMove -= ArriveLane;
        _controller = null;
    }

    public void HandleEvent(StateEvent stateEvent)
    {
        switch (stateEvent)
        {
            case OnTriggerEnterEvent:
                _isHit = true;
                _controller.LaneMover.MoveLane(_direction * -1, true);
                break;

            case ParriedEvent:
                _controller.KnockbackToSide(_direction * -1);

                break;
        }
    }

    public void Update()
    {
    }

    public void ArriveLane()
    {
        if (_isHit)
        {
            _controller.OnChaseEnd();

        }
        else
        {
            _controller.LaneMover.MoveLane(_direction);
        }
    }
}

public class SideKnockbackState : IState<TrailerTruckController>
{
    private TrailerTruckController _controller;
    private LaneMover _laneMover;
    private float _knockbackSpeed;
    public void Enter(TrailerTruckController owner)
    {
        _controller = owner;
        _laneMover = _controller.LaneMover;
        _laneMover.UpdateMoveLaneSpeed(_knockbackSpeed);
        _laneMover.OnFinishMove += ArriveLane;
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }

    public void ArriveLane()
    {
        _controller.ChangeMatchState();
    }

    public void HandleEvent(StateEvent stateEvent)
    {
    }
}

public class ForwardKnockbackState : IState<TrailerTruckController>
{
    TrailerTruckController _controller;
    public void Enter(TrailerTruckController owner)
    {
        _controller = owner;
        owner.KnockbackToForward();
    }

    public void Exit()
    {
        _controller = null;
    }

    public void HandleEvent(StateEvent stateEvent)
    {
    }

    public void Update()
    {
        _controller.ApplyFriction();
        _controller.RecoverVelocity();
    }
}