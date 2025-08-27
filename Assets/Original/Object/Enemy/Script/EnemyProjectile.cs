using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile: MonoBehaviour, IParryable
{
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private string _LayerNameAfterParry;

    private Vector3 _forwardVector = Vector3.right;
    private Vector3 _rotateDirection = new Vector3(0f, 0.5f, 0f);
    private float _curVelocity;
    private bool _isParried;

    private void FixedUpdate()
    {
        if (_isParried == false)
        {
            _curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        }

        Vector3 targetPosition = _rigidbody.position + (_forwardVector * _curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    public void OnParried(Vector3 contactPosition, float damage, float moveLaneSpeed)
    {
        _isParried = true;
        _curVelocity = GlobalMovementController.Instance.GlobalVelocity * -1.5f;
        _rigidbody.AddRelativeTorque(Vector3.right * 1000f, ForceMode.Force);
    }

    public void OnAttack()
    {
    }
}