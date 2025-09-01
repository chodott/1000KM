using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Text countdownText;   // 카운트다운 보여줄 UI Text
    public float interval = 1f;  // 숫자 줄어드는 간격 (초)

    void OnEnable()
    {
        Time.timeScale = 0f; // 게임을 멈춘 상태에서 카운트다운 시작
        StartCoroutine(CountRoutine());
    }

    IEnumerator CountRoutine()
    {
        int count = 3;

        while (count > 0)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();

            yield return new WaitForSecondsRealtime(interval);
            count--;
        }

        // 카운트가 끝나면
        if (countdownText != null)
            countdownText.text = "";

        Time.timeScale = 1f;      // 다시 게임 진행
        gameObject.SetActive(false); // 자기 자신 비활성화
    }
}
