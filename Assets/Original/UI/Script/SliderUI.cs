using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float _maxValue = 100.0f;

    private float _curValue = 50.0f;

    private void Start()
    {
        slider.maxValue = _maxValue;
    }
    public void UpdateValue(float value)
    {
        _curValue = value;
        slider.value = _curValue;
    }

    public float GetMaxValue()
    {
        return _maxValue;
    }

    public float GetCurValue()
    {
        return _curValue;
    }
}
