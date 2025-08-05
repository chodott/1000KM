using UnityEngine;

public class GlobalMovementController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    public static GlobalMovementController Instance { get; private set; }
    public float globalVelocity { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void Init(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
        _playerMovement.OnSpeedChanged += HandleVelocityChanged;
    }

    void OnDestroy()
    {
        _playerMovement.OnSpeedChanged -= HandleVelocityChanged;
    }

    private void HandleVelocityChanged(float speed)
    {
        globalVelocity = speed;
    }
}
