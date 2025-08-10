using System;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    public event Action<Collider> InParryRangeEvent;

    [SerializeField]
    private BoxCollider _collider;

    private void Start()
    {
        SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        InParryRangeEvent?.Invoke(other);
    }

    public void SetActive(bool active)
    {
        _collider.enabled = active;
    }

}
