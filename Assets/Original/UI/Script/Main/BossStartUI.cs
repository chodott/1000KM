using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossStartUI : MonoBehaviour
{
    public float blinkDuration = 0.5f;  // 한번 깜빡이는 시간
    public int blinkCount = 3;          // 몇 번 깜빡일지
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        // 시작 시 투명
        canvasGroup.alpha = 0f;
        StopAllCoroutines();
        StartCoroutine(BlinkAndDisable());
    }

    IEnumerator BlinkAndDisable()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // 밝아지기
            yield return StartCoroutine(FadeAlpha(0f, 1f, blinkDuration / 2f));
            // 어두워지기
            yield return StartCoroutine(FadeAlpha(1f, 0f, blinkDuration / 2f));
        }

        // 끝나면 자동으로 비활성화
        gameObject.SetActive(false);
    }

    IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
