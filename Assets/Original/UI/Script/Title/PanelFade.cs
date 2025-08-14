using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    public float fadeDuration = 1f;
    public bool fadeInOnEnable = true; // true=페이드인, false=페이드아웃

    Image panelImage;

    void Awake()
    {
        // 같은 오브젝트에 붙은 Image 자동 찾기
        panelImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        StopAllCoroutines();

        if (fadeInOnEnable)
        {
            // 알파 0에서 시작 → 페이드인
            Color c = panelImage.color;
            c.a = 0f;
            panelImage.color = c;
            StartCoroutine(FadeIn());
        }
        else
        {
            // 알파 1에서 시작 → 페이드아웃 후 비활성화
            Color c = panelImage.color;
            c.a = 1f;
            panelImage.color = c;
            StartCoroutine(FadeOutAndDisable());
        }
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = panelImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            panelImage.color = c;
            yield return null;
        }

        c.a = 1f;
        panelImage.color = c;
    }

    IEnumerator FadeOutAndDisable()
    {
        float elapsed = 0f;
        Color c = panelImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            panelImage.color = c;
            yield return null;
        }

        c.a = 0f;
        panelImage.color = c;

        gameObject.SetActive(false); // 페이드아웃 끝나면 비활성화
    }
}
