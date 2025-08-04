using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    SliderUI _gasSlider;
    [SerializeField]
    SliderUI _hpSlider;

    public void Init(PlayerStatus playerStatus)
    {
        playerStatus.OnGasPointChanged += UpdateGasUI;
        playerStatus.OnHPChanged += UpdateHpUI;
    }

    private void UpdateGasUI(float value)
    {
        _gasSlider.UpdateValue(value);
    }

    private void UpdateHpUI(float value)
    {
        _hpSlider.UpdateValue(value);
    }
}
