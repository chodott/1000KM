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
        _uiManager.Init(playerStatus);
    }

}
