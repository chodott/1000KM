using System;
using UnityEngine;

public class RestAreaEntrance : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _boxCollider;

    private void OnEnable()
    {
        GlobalMovementController.Instance.OnRestAreaNearby += Activate;
    }

    private void Update()
    {
        float curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        transform.position += (Vector3.right * curVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            player.DeactivateInput();
        }
    }
    private void Activate()
    {
        _boxCollider.enabled = true;
    }

    private void Deactivate()
    {
        _boxCollider.enabled = false;
    }
}