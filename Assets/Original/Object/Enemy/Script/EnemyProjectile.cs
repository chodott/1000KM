using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile: MonoBehaviour, IParryable
{
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Collider _triggerCollider;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private GameObject _explosionEffectPrefab;
    [SerializeField]
    private string _LayerNameAfterParry;
    [SerializeField]
    private bool _canParry;

    private Vector3 _forwardVector = Vector3.right;
    private float _curVelocity;
    private bool _isParried;

    private void OnEnable()
    {
        _triggerCollider.enabled = false;
    }
    private void FixedUpdate()
    {
        if (_isParried == false)
        {
            _curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        }

        Vector3 targetPosition = _rigidbody.position + (_forwardVector * _curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    private void SpawnExplosionEffect()
    {
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            return;
        }

        if(other.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.OnDamaged(1);
            SpawnExplosionEffect();
            Destroy(this.gameObject);
        }
    }

    public void OnParried(Vector3 contactPosition, float damage, float moveLaneSpeed)
    {
        if(_canParry)
        {
            _isParried = true;
            _triggerCollider.enabled = true;
            _triggerCollider.gameObject.layer = LayerMask.NameToLayer(_LayerNameAfterParry);
            _curVelocity = GlobalMovementController.Instance.GlobalVelocity * -1.5f;
            _rigidbody.AddRelativeTorque(Vector3.right * 1000f, ForceMode.Force);
        }
        else
        {
            SpawnExplosionEffect();
        }
    }

    public void OnAttack()
    {
    }
}