using UnityEngine;

public class BaseEnemy : MonoBehaviour 
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private BoxCollider _collider;

    private Vector3 _forwardVector = -Vector3.right;

    protected float _curHealthPoint;
    protected float _curVelocity;
    protected float _maxVelocity;

    protected float _friction = 1f;
    protected float _knockbackPower = 0.5f;
    protected float _parryPower = 50f;

    private DriveState _driveState = new DriveState();
    public DriveState DriveState { get { return _driveState; } }

    public LaneMover LaneMover { get { return _laneMover; } }
    public Rigidbody Rb { get { return _rigidbody; } }
    public BoxCollider Collider { get { return _collider; } }

    protected virtual void FixedUpdate()
    {
        MoveForward(_curVelocity);
    }

    private void MoveForward(float velocity)
    {
        float curVelocity = velocity - GlobalMovementController.Instance.GlobalVelocity;
        Vector3 targetPosition = _rigidbody.position + (_forwardVector * curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    public void ApplyFriction()
    {
        if (_curVelocity <= _maxVelocity)
        {
            _curVelocity = _maxVelocity;
        }
        else
        {
            _curVelocity -= _friction * Time.fixedDeltaTime * _curVelocity;
        }
    }

    public void ResetVelocity()
    {
        _curVelocity = 0f;
    }


    public void KnockbackToForward()
    {
        _curVelocity += (GlobalMovementController.Instance.GlobalVelocity * (1 + _knockbackPower));
    }

    public virtual void KnockbackToSide(Vector3 parriedDirection, float sign)
    {
        LaneMover.KnockbackLane(sign);
    }

}
