using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _defaultAcceleration = 5.0f;
    [SerializeField]
    private float _defaultDrag = 1.5f;
    [SerializeField]
    private float _lockVelocity = 80f;

    private float _acceleration;
    private float _velocity;
    private float _keyValue;
    private float _drag;
    private bool _isLockInput = false;

    public event Action<float> OnSpeedChanged;

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
            _velocity += _acceleration * Time.deltaTime;
        }

        else if (_keyValue < 0f)
        {
            _velocity -= _acceleration * Time.deltaTime;
        }

        else
        {
            _velocity -= _drag * Time.deltaTime;
        }

        if(_velocity <=0f)
        {
            _velocity = 0f;
        }
            OnSpeedChanged?.Invoke(_velocity);
    }

    IEnumerator LerpLockVelocity(float from, float to, float duration)
    {
        float totalDist = Mathf.Abs(to - from);
        float speedPerSec = (duration > 0f) ? (totalDist / duration) : float.PositiveInfinity;

        _velocity = from;
        while (!Mathf.Approximately(_velocity, to))
        {
            _velocity = Mathf.MoveTowards(_velocity, to, speedPerSec * Time.deltaTime);
            OnSpeedChanged?.Invoke(_velocity);
            yield return null;
        }
        _velocity = to;
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

        _velocity /= 2;
    }

    public void SetMoveDirection(float value)
    {
        _keyValue = value;
;    }

    public void LockAcceleration()
    {
        _isLockInput = true;
        StartCoroutine(LerpLockVelocity(_velocity, _lockVelocity, 5f));
    }

}
