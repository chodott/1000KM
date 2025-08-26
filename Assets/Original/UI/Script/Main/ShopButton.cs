using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    private PlayerStatus _playerStatus;
    public PartStatus partStatus;
    public int payment;
    Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = payment + " ¿ø";
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
        }
    }

    public void OnClickRepairButton()
    {
        int fee = 100;
        float healAmount = 100f;
        if (_playerStatus.TrySpendMoney(fee) == true)
        {
            _playerStatus.RefillHP(healAmount);
        }
    }

    public void OnClickPartButton()
    {

    }
}
