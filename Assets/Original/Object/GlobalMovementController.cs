using System;
using UnityEngine;

public class GlobalMovementController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    public static GlobalMovementController Instance { get; private set; }

    public float GlobalVelocity { get; private set; }
    public float TotalDistance { get; private set; }

    #region Monobehaviour Callbacks
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        _playerMovement.OnSpeedChanged -= HandleVelocityChanged;
    }

    private void FixedUpdate()
    {
        TotalDistance += GlobalVelocity * Time.fixedDeltaTime;
        Debug.Log($"totalDistance: {TotalDistance}");
    }
    #endregion

    public void Init(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
        _playerMovement.OnSpeedChanged += HandleVelocityChanged;
    }



    private void HandleVelocityChanged(float velocity)
    {
        GlobalVelocity = velocity;
    }
}
