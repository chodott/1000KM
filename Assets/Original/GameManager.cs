using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    UIManager _uiManager;
    [SerializeField]
    Player _player;
    #endregion

    private void Start()
    {
        PlayerStatus playerStatus = _player.GetComponent<PlayerStatus>();
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
        _uiManager.Init(playerStatus, playerMovement);
    }

}
