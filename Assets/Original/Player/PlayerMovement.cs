using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _acceleration = 5.0f;
    [SerializeField]
    private float _moveStepSize = 5.0f;
    [SerializeField]
    private float _moveStepSpeed = 10.0f;
    [SerializeField]
    private float _stopDistance = 0.03f;
    [SerializeField]
    private int _laneRange = 1;

    private Vector3 _nextPosition;
    private Vector3 _rightVector = new Vector3(0,0,1);
    private float _speed;
    private float _keyValue;
    private int _currentLaneIndex = 0;
    private bool _isMoving = false;

    public event Action<float> OnSpeedChanged;

    private void Update()
    {
        Accelerate();

        if (_isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, _nextPosition, Time.deltaTime * _moveStepSpeed);
            if (Vector3.Distance(transform.position, _nextPosition) <= _stopDistance)
            {
                transform.position = _nextPosition;
                _isMoving = false;
            }
        }  
    }

    private void Accelerate()
    {
        if (_keyValue < 0f)
        {
            _speed += _acceleration * Time.deltaTime;
        }

        else if (_keyValue < 0f)
        {
            _speed -= _acceleration * Time.deltaTime;
        }
        OnSpeedChanged?.Invoke(_speed);
    }

    private bool GetCanMove(int direction)
    {
        if (_isMoving)
        {
            return false;
        }

        int maxLaneIndex = direction * _laneRange;
        if (_currentLaneIndex == maxLaneIndex)
        {
            return false;
        }

        return true;
    }

    public void SetMoveDirection(float value)
    {
        _keyValue = value;
    }

    public void MoveHorizon(float isRight)
    {

        int direction = Math.Sign(isRight);
        if(GetCanMove(direction) == false)
        {
            return;
        }
        _currentLaneIndex += direction;
        _nextPosition = transform.position + (_rightVector * isRight * _moveStepSize);
        _isMoving = true;
    }
}
