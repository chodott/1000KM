using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    SliderUI _gasSlider;
    [SerializeField]
    SliderUI _hpSlider;
    [SerializeField]
    SliderUI _toiletSlider;
    [SerializeField]
    SliderUI _speedSlider;
    [SerializeField]
    ShopButton _shopUI;

    public void Init(PlayerStatus playerStatus, PlayerMovement playerMovement)
    {
        playerStatus.OnGasPointChanged += UpdateGasUI;
        playerStatus.OnHPChanged += UpdateHpUI;
        playerStatus.OnToiletPointChanged += UpdateToiletUI;
        playerMovement.OnSpeedChanged += UpdateSpeedUI;

        _shopUI.Init(playerStatus);

    }

    private void UpdateGasUI(float value)
    {
        _gasSlider.UpdateValue(value);
    }

    private void UpdateHpUI(float value)
    {
        _hpSlider.UpdateValue(value);
    }

    private void UpdateToiletUI(float value)
    {
        _toiletSlider.UpdateValue(value);
    }
    private void UpdateSpeedUI(float value)
    {
        _speedSlider.UpdateValue(value);
    }
}
