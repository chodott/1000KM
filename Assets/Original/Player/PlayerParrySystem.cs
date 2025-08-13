using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParrySystem : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private GameObject _parryEffectPrefab;
    [SerializeField]
    private ParryCollider _parryCollider;
    [SerializeField]
    private float _parryCooldownTime = 0.3f;
    [SerializeField]
    private int _spinCount = 3;
    #endregion

    private Quaternion _baseRotation;
    private float _parryPower = 1f;
    private bool _isParry = false;

    private void Start()
    {
       _baseRotation = transform.rotation;
        _parryCollider.InParryRangeEvent += ParrySuccess;
    }

    private void ParrySuccess(Collider other)
    {
        if (other.TryGetComponent<IParryable>(out var parryable))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Instantiate(_parryEffectPrefab, contactPoint, Quaternion.identity);
            Vector3 parryDirection = (other.transform.position - transform.position).normalized;

            parryable.OnParried(contactPoint, _parryPower);
        }
    }

    private IEnumerator Spin()
    {
        _isParry = true;
        _parryCollider.SetActive(true);


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
        _parryCollider.SetActive(false);

    }

    public void Parry()
    {
        if (_isParry == false)
        {
            StartCoroutine(Spin());
        }
    }
}
