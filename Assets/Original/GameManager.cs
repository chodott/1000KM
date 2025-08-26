using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    UIManager _uiManager;
    [SerializeField]
    Player _player;
    #endregion

    private void OnEnable()
    {
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
        _uiManager.Init(_player.Status, playerMovement);
        GlobalMovementController.Instance.Init(playerMovement);
    }
}
