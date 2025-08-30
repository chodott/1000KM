using System;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public bool IsActive { get { return _collider.enabled; } }

    [SerializeField]
    private BoxCollider _collider;

    public void SetActive(bool activate)
    {
        _collider.enabled = activate;
    }
}
