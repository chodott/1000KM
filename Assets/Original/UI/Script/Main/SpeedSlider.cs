using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    public GameObject needle;
    public Text speedValue;

    Slider slider;
    SliderUI sliderUI;
    float startAngle = 12f;
    float endAngle = -192f;
    float maxValue;
    float effectSpeed = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        sliderUI = GetComponent<SliderUI>();
        maxValue = sliderUI.GetMaxValue();
    }

    void Update()
    {
        UpdateValueText();
        UpdateNeelde();
    }

    void UpdateValueText()
    {
        speedValue.text =  Mathf.FloorToInt(sliderUI.GetCurValue() * 3.6f) + "km/h";

        float t = slider.value / maxValue;

        Color color;

        if (t < 1f / 3f) // ∆ƒ∂˚ -> √ ∑œ
        {
            float lerpT = t / (1f / 3f);
            color = Color.Lerp(Color.blue, Color.green, lerpT);
        }
        else if (t < 2f / 3f) // √ ∑œ -> ≥Î∂˚
        {
            float lerpT = (t - 1f / 3f) / (1f / 3f);
            color = Color.Lerp(Color.green, Color.yellow, lerpT);
        }
        else // ≥Î∂˚ -> ª°∞≠
        {
            float lerpT = (t - 2f / 3f) / (1f / 3f);
            color = Color.Lerp(Color.yellow, Color.red, lerpT);
        }

        speedValue.color = color;
    }

    void UpdateNeelde()
    {
        if (sliderUI.GetCurValue() >= maxValue - 0.001f)
        {
            float t = (Mathf.Sin(Time.time * (effectSpeed + (sliderUI.GetCurValue() - maxValue) * 0.1f)) + 1f) / 2f;
            float angle = Mathf.Lerp(-182f, -202f, t);
            needle.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            float t = slider.value / slider.maxValue;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            needle.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
