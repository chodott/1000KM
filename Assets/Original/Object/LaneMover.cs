using System;
using Unity.VisualScripting;
using UnityEngine;

public class LaneMover : MonoBehaviour
{
    [SerializeField]
    private float _moveLaneSpeed;
    [SerializeField]
    private float _stopDistance = 0.05f;

    private Rigidbody _rigidbody;
    private float _nextPositionZ;
    private float _laneWidth;
    private int _currentLaneIndex = 0;
    private bool _isMoving = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _laneWidth = LaneSystem.Instance.LaneWidth;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            float curPositionZ = Mathf.Lerp(_rigidbody.position.z, _nextPositionZ, Time.deltaTime * _moveLaneSpeed);
            Vector3 newPosition = new Vector3(_rigidbody.position.x, _rigidbody.position.y, curPositionZ);

            _rigidbody.MovePosition(newPosition);

            if (Mathf.Abs(curPositionZ - _nextPositionZ) <= _stopDistance)
            {
                Vector3 finalPosition = new Vector3(_rigidbody.position.x, _rigidbody.position.y, _nextPositionZ);
                _rigidbody.MovePosition(finalPosition);
                _isMoving = false;
            }
        }
    }
    public void Init(int laneIndex)
    {
        _currentLaneIndex = laneIndex;
    }
    public bool MoveLane(float isRight)
    {
        if(_isMoving)
        {
            return false;
        }

        int direction = Math.Sign(isRight);
        bool canMove = LaneSystem.Instance.GetCanMove(direction, _currentLaneIndex);
        if (canMove == false)
        {
            return false;
        }
        _nextPositionZ = transform.position.z + (_laneWidth * isRight);
        _currentLaneIndex += direction;
        _isMoving = true;
        return true;
    }

    public void CheckAndMoveLane(Vector3 position, Vector3 colliderSize, float isRight)
    {
        Vector3 checkPosition = new Vector3(position.x, position.y, position.z + _laneWidth * isRight);
       if( Physics.CheckBox(checkPosition, colliderSize, Quaternion.Euler(0,-90f,0), LayerMask.GetMask("Enemy")))
        {
            return;
        }

        MoveLane(isRight);
    }
}
