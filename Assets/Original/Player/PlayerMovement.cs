using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _defaultAcceleration = 5.0f;
    [SerializeField]
    private float _defaultDrag = 1.5f;

    private float _acceleration;
    private float _velocity;
    private float _keyValue;
    private float _drag;
    private bool _isLockInput = false;

    public event Action<float> OnSpeedChanged;
    public float Velocity { get { return _velocity; } 
        private set 
        {
            _velocity = value;
            OnSpeedChanged?.Invoke(_velocity); 
        }
    }

    private void Update()
    {
        Accelerate();
    }

    private void Accelerate()
    {
        if(_isLockInput)
        {
            return;
        }

        if (_keyValue > 0f)
        {
            Velocity += _acceleration * Time.deltaTime;
        }

        else if (_keyValue < 0f)
        {
            Velocity -= _acceleration * Time.deltaTime;
        }

        else
        {
            Velocity -= _drag * Time.deltaTime;
        }

        if(_velocity <=0f)
        {
            Velocity = 0f;
        }
    }

    IEnumerator LerpLockVelocity(float from, float to, float duration)
    {
        float totalDist = Mathf.Abs(to - from);
        float speedPerSec = (duration > 0f) ? (totalDist / duration) : float.PositiveInfinity;

        Velocity = from;
        while (!Mathf.Approximately(Velocity, to))
        {
            Velocity = Mathf.MoveTowards(Velocity, to, speedPerSec * Time.deltaTime);
            yield return null;
        }
        Velocity = to;
    }

    public void UpdateStatus(PartStatus status)
    {
        _acceleration = _defaultAcceleration + status.AccelerationBonus;
        _drag = _defaultDrag - status.DragReduction;
    }

    public void OnDamaged()
    {
        if(_isLockInput)
        {
            return;
        }

        Velocity /= 2;
    }

    public void SetMoveDirection(float value)
    {
        _keyValue = value;
;    }

    public void LockAcceleration(float lockVelocity, float duration)
    {
        _isLockInput = true;
        StartCoroutine(LerpLockVelocity(_velocity, lockVelocity, duration));
    }

    public void LockAcceleration()
    {
        _isLockInput = true;
    }

    public void UnlockAcceleration()
    {
        _isLockInput = false;
    }

}
