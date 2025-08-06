using System;
using Unity.VisualScripting;
using UnityEngine;

public class LaneMover : MonoBehaviour
{
    static private Vector3 _rightVector = new Vector3(0, 0, 1);
    [SerializeField]
    private float _moveLaneSpeed;
    [SerializeField]
    private float _stopDistance = 0.05f;

    private float _nextPositionZ;
    private int _currentLaneIndex = 0;
    private bool _isMoving = false;

    private void Update()
    {
        if (_isMoving)
        {
            
            float curPositionZ = Mathf.Lerp(transform.position.z, _nextPositionZ, Time.deltaTime * _moveLaneSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, curPositionZ);
            if (Mathf.Abs(curPositionZ - _nextPositionZ)  <= _stopDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, _nextPositionZ);
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
        _nextPositionZ = transform.position.z + (LaneSystem.Instance.LaneWidth * isRight);
        _currentLaneIndex += direction;
        _isMoving = true;
    }
}
