using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _acceleration = 5.0f;

    private float _speed;
    private float _keyValue;

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

    public void SetMoveDirection(float value)
    {
        _keyValue = value;
;    }
}
