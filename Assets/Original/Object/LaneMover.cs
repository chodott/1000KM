using System;
using UnityEngine;

public class LaneMover : MonoBehaviour
{
    static private Vector3 _rightVector = new Vector3(0, 0, 1);
    [SerializeField]
    private float _moveLaneSpeed;
    [SerializeField]
    private float _stopDistance = 0.05f;

    private Vector3 _nextPosition;
    private int _currentLaneIndex = 0;
    private bool _isMoving = false;

    private void Update()
    {
        if (_isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, _nextPosition, Time.deltaTime * _moveLaneSpeed);
            if (Vector3.Distance(transform.position, _nextPosition) <= _stopDistance)
            {
                transform.position = _nextPosition;
                _isMoving = false;
            }
        }
    }
    public void MoveLane(float isRight)
    {
        if(_isMoving)
        {
            return;
        }

        int direction = Math.Sign(isRight);
        bool canMove = LaneSystem.Instance.GetCanMove(direction, _currentLaneIndex);
        if (canMove == false)
        {
            return;
        }
        _nextPosition = transform.position + (_rightVector * LaneSystem.Instance.LaneWidth * isRight);
        _currentLaneIndex += direction;
        _isMoving = true;
    }
}
