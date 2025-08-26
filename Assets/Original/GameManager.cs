using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    UIManager _uiManager;
    [SerializeField]
    Player _player;
    #endregion

    private int _curMoney;
    public int CurrentMoney;

    public void EarnMoney(int amount)
    {
        _curMoney += amount;
    }

    private void OnEnable()
    {
        PlayerStatus playerStatus = _player.GetComponent<PlayerStatus>();
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
        _uiManager.Init(playerStatus, playerMovement);
        GlobalMovementController.Instance.Init(playerMovement);

        CarEnemy.OnRewardDropped += EarnMoney;
    }

    private void OnDisable()
    {
        CarEnemy.OnRewardDropped -= EarnMoney;
    }

}
