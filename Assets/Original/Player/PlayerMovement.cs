using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _acceleration = 5.0f;
    [SerializeField]
    private float _moveStepSize = 10.0f;
    [SerializeField]
    private float _moveStepSpeed = 10.0f;

    private Vector3 _nextPosition;
    private float _speed;
    private float _keyValue;


    private void Update()
    {
        Accelerate();
        transform.position = Vector3.Lerp(transform.position, _nextPosition, Time.deltaTime * _moveStepSpeed);

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
    }

    public void SetMoveDirection(float value)
    {
        _keyValue = value;
    }

    public void MoveHorizon(float isRight)
    {
        _nextPosition = transform.position + (transform.right * isRight * _moveStepSize);

    }
}
