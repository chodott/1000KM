using System;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    public event Action<Collider> InParryRangeEvent;

    [SerializeField]
    private CapsuleCollider _collider;

    private void Start()
    {
        SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Default") == false)
        {
            return;
        }

        InParryRangeEvent?.Invoke(other);
    }

    public void SetActive(bool active)
    {
        _collider.enabled = active;
    }

}
