using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LaneMover : MonoBehaviour
{
    [SerializeField]
    private Animation _animation; 
    [SerializeField]
    private AnimationClip _moveLeftAnimation;
    [SerializeField]
    private AnimationClip _moveRightAnimation;
    [SerializeField]
    private AnimationClip _knockbackLeftAnimation;
    [SerializeField]
    private AnimationClip _knockbackRightAnimation;

    [SerializeField]
    private float _defaultMoveLaneSpeed;
    [SerializeField]
    private float _stopDistance = 0.05f;


    public event Action OnFinishMove;
    private Rigidbody _rigidbody;
    private float _moveLaneSpeed;
    private float _nextPositionZ;
    private float _laneWidth;
    private int _currentLaneIndex = 0;
    private bool _isMoving = false;

    public float MoveLaneSpeed { get { return _moveLaneSpeed; } }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _laneWidth = LaneSystem.Instance.LaneWidth;
        _moveLaneSpeed = _defaultMoveLaneSpeed;
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

                OnFinishMove?.Invoke();
            }
        }
    }

    private void SetTargetLanePosition(int isRight)
    {
        _nextPositionZ = transform.position.z + (_laneWidth * isRight);
        _currentLaneIndex += isRight;
        _isMoving = true;
    }
    public void Init(int laneIndex)
    {
        _currentLaneIndex = laneIndex;
    }

    public void StopLaneMove()
    {
        _nextPositionZ = 0f;
        _isMoving = false;
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

        SetTargetLanePosition(direction);

        AnimationClip playClip = direction > 0 ? _moveRightAnimation : _moveLeftAnimation;
        _animation.Play(playClip.name);
        return true;
    }

    public bool KnockbackLane(float isRight)
    {
        int direction = Math.Sign(isRight);
        bool canMove = LaneSystem.Instance.GetCanMove(direction, _currentLaneIndex);
        if(canMove == false)
        {
            return false;
        }

        SetTargetLanePosition(direction);
        AnimationClip playClip = direction > 0 ? _knockbackRightAnimation : _knockbackLeftAnimation;
        _animation.Play(playClip.name);
        return true;
    }

    public void UpdateMoveLaneSpeed(float bonus)
    {
        _moveLaneSpeed = _defaultMoveLaneSpeed + bonus;
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
