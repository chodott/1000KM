using System;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public event Action<Collider> OnHitEvent;

    [SerializeField]
    private BoxCollider _collider;

    private void OnTriggerEnter(Collider other)
    {
        OnHitEvent?.Invoke(other);
    }

}
