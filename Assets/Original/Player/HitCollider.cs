using System;
using UnityEngine;

public class HitCollider : MonoBehaviour, IDamagable
{
    [SerializeField]
    private BoxCollider _collider;

    public event Action<float> OnHitEvent;

    public void OnDamaged(float amount)
    {
        OnHitEvent?.Invoke(amount);
    }
}
