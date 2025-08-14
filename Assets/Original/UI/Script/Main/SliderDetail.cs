using UnityEngine;
using UnityEngine.UI;

public class SliderDetail : MonoBehaviour
{
    [SerializeField]
    private Slider sliderHP;
    [SerializeField]
    private Slider sliderGas;
    [SerializeField]
    private Slider sliderToilet;
    private SliderUI[] sliderUI;

    [SerializeField]
    private Text[] texts;

    private void Start()
    {
        sliderUI = new SliderUI[3];
        sliderUI[0] = sliderHP.GetComponent<SliderUI>();
        sliderUI[1] = sliderGas.GetComponent<SliderUI>();
        sliderUI[2] = sliderToilet.GetComponent<SliderUI>();
    }


    private void Update()
    {
        texts[0].text = "H : " + RoundTo1Decimal(sliderUI[0].GetCurValue()).ToString("000.0") + "/" + sliderUI[0].GetMaxValue().ToString("000.0");
        texts[1].text = "G : " + RoundTo1Decimal(sliderUI[1].GetCurValue()).ToString("000.0") + "/" + sliderUI[1].GetMaxValue().ToString("000.0");
        texts[2].text = "T : " + RoundTo1Decimal(sliderUI[2].GetCurValue()).ToString("000.0") + "/" + sliderUI[2].GetMaxValue().ToString("000.0");
    }

    float RoundTo1Decimal(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }
}
