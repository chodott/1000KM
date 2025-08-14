using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    public float fadeDuration = 1f;

    Image panelImage;

    void Awake()
    {
        // 같은 오브젝트에 붙은 Image 자동 찾기
        panelImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        Color c = panelImage.color;
        c.a = 0f;
        panelImage.color = c;

        StopAllCoroutines();
        StartCoroutine(FadeIn());
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
}
