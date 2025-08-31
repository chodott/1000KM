using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    AudioSource buttoClickSound;
    private PlayerStatus _playerStatus;
    [SerializeField] PartStatus[] ps;
    [SerializeField] PartManager pm;
    [SerializeField] Button oilButton;
    [SerializeField] Button repairButton;
    [SerializeField] Button[] partsButtons;
   

    private void Start()
    {
        buttoClickSound = GetComponent<AudioSource>();
        oilButton.onClick.AddListener(OnClickOilButton);
        repairButton.onClick.AddListener(OnClickRepairButton);
        for (int i = 0; i < partsButtons.Length; i++)
        {
            int idx = i;
            partsButtons[i].onClick.AddListener(() => OnClickPartButton(idx));
        }
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
            buttoClickSound.Play();
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
            buttoClickSound.Play();
        }
    }

    public void OnClickPartButton(int i)
    {
        int fee = 200;
        if (_playerStatus.TrySpendMoney(fee) == true)
        {
            _playerStatus.CurrentMoney -= fee;
            buttoClickSound.Play();
            partsButtons[i].GetComponent<PartsButton>().AddStatus();
        }
    }
}
