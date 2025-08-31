using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float _maxValue = 100.0f;

    private float _curValue = 50.0f;

    private void OnEnable()
    {
        slider.maxValue = _maxValue;
    }
    public void UpdateValue(float value)
    {
        _curValue = value;
        StopAllCoroutines();
        StartCoroutine(SmoothChange());
    }

    public float GetMaxValue()
    {
        return _maxValue;
    }

    public float GetCurValue()
    {
        return _curValue;
    }

    IEnumerator SmoothChange()
    {
        float startValue = slider.value;
        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, _curValue, time / duration);
            yield return null;
        }

        slider.value = _curValue;
    }
}
