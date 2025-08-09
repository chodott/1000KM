using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParrySystem : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private float _parryCooldownTime = 0.3f;
    [SerializeField]
    private int _spinCount = 3;

    private Quaternion _baseRotation;
    private float _parryForce = 50f;
    private float _parryPower = 1f;
    private bool _isParry = false;

    private void Start()
    {
       _baseRotation = transform.rotation;
    }

    #region Collider Callbacks
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IParryable>(out var parryable))
        {
            Vector3 parryDirection = (other.transform.position - transform.position).normalized;
            parryable.OnParried(parryDirection, _parryForce, _parryPower);
            _boxCollider.enabled = false;

        }
    }
    #endregion

    private IEnumerator Spin()
    {
        _isParry = true;
        _boxCollider.enabled = true;

        float elapsedTime = 0f;
        float totalSpinAmount = 360f * _spinCount;

        while(elapsedTime < _parryCooldownTime)
        {
            float deltaAngle = (totalSpinAmount / _parryCooldownTime) * Time.deltaTime;
            transform.Rotate(0, deltaAngle, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isParry = false;
        transform.rotation = _baseRotation;
    }

    public void Parry()
    {
        if (_isParry == false)
        {
            StartCoroutine(Spin());
        }
    }
}
