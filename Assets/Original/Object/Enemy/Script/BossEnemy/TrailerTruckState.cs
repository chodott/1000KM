
using UnityEngine;

public class PressState : IState<TrailerTruckController>
{
    TrailerTruckController _controller;
    private float _keepDistance = 6.5f;
    public void Enter(TrailerTruckController owner)
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void OnCollisionEnter(Collision collision)
    {
        _controller.ChangeMatchState();
    }

    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        _controller.MoveToBack();
        if (_controller.GetXDistanceToPlayer() <= _keepDistance)
        {
            _controller.OnMatchGapEnd();
        }
    }
}

public class ChaseState : IState<TrailerTruckController>
{
    public void Enter(TrailerTruckController owner)
    {
        float offset = owner.GetZOffsetToPlayer();
        float direction = (int)Mathf.Sign(offset);

        owner.LaneMover.MoveLane(direction, 5);
    }

    public void Exit()
    {
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
}