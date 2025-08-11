using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _defaultAcceleration = 5.0f;
    [SerializeField]
    private float _defaultDrag = 1f;

    private float _acceleration;
    private float _speed;
    private float _keyValue;
    private float _drag;

    public event Action<float> OnSpeedChanged;

    private void Update()
    {
        Accelerate();
    }

    private void Accelerate()
    {
        if (_keyValue > 0f)
        {
            _speed += _acceleration * Time.deltaTime;
        }

        else if (_keyValue < 0f)
        {
            _speed -= _acceleration * Time.deltaTime;
        }
        OnSpeedChanged?.Invoke(_speed);
    }

    public void UpdateStatus(PartStatus status)
    {
        _acceleration = _defaultAcceleration + status.AccelerationBonus;
        _drag = _defaultDrag - status.DragReduction;
    }

    public void SetMoveDirection(float value)
    {
        _keyValue = value;
;    }
}
