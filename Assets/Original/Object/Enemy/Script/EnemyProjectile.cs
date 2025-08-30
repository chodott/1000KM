using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile: MonoBehaviour
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
    private float _lifeTime = 5f;
    [SerializeField]
    private float _returnPositionX = 5f;
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
        if(_rigidbody.position.x >= _returnPositionX)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnExplosionEffect()
    {
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
    }

    public void OnTriggerEnter(Collider other)
    {
        var host = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (host.TryGetComponent<IDamagable>(out var damagable))
        {
            bool result = damagable.OnDamaged(1);
            if(result == true)
            {
                SpawnExplosionEffect();
                Destroy(this.gameObject);
            }
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
            StartCoroutine(DestroyAfterDelay(_lifeTime));
        }
        else
        {
            SpawnExplosionEffect();
            Destroy(this.gameObject);
        }
    }
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

}