using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    private PlayerStatus _playerStatus;
    [SerializeField] Button oilButton;
    [SerializeField] Button repairButton;

    private void Start()
    {
        oilButton.onClick.AddListener(OnClickOilButton);
        repairButton.onClick.AddListener(OnClickRepairButton);
    }

    public void Init(PlayerStatus playerStatus)
    {
        _playerStatus = playerStatus;
    }

    public void OnClickOilButton()
    {
        int fee = 100;
        if(_playerStatus.TrySpendMoney(fee) == true)
        {
            _playerStatus.RefillGas();
            _playerStatus.CurrentMoney -= fee;
        }
    }

    public void OnClickRepairButton()
    {
        int fee = 100;
        float healAmount = 100f;
        if (_playerStatus.TrySpendMoney(fee) == true)
        {
            _playerStatus.RefillHP(healAmount);
            _playerStatus.CurrentMoney -= fee;
        }
    }

    public void OnClickPartButton()
    {

    }
}
