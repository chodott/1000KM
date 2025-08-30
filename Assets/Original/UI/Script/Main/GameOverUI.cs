using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] CanvasGroup group;              // GameOver 패널에 붙은 CanvasGroup
    [SerializeField] Text pressAnyKeyText;           // "Press any key..." 텍스트 (선택)

    [Header("Behavior")]
    [SerializeField] float fadeDuration = 1.0f;      // 페이드 인 시간 (unscaled)
    [SerializeField] bool pauseAudio = true;         // 게임오버 시 오디오 일시정지 여부

    bool readyForInput = false;
    Coroutine blinkRoutine;

    void Reset()
    {
        group = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        // 시작 상태
        if (group != null)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = true; // 클릭 막기
        }

        if (pauseAudio) AudioListener.pause = true;

        // 안내 텍스트 초기화
        if (pressAnyKeyText != null)
            pressAnyKeyText.enabled = false;

        readyForInput = false;

        // ?? 활성화되자마자 페이드 시작
        StartCoroutine(FadeIn());
    }

    void OnDisable()
    {
        if (blinkRoutine != null) StopCoroutine(blinkRoutine);
        if (pressAnyKeyText != null) pressAnyKeyText.enabled = false;
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        float from = 0f, to = 1f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            if (group != null) group.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        if (group != null)
        {
            group.alpha = 1f;
            group.interactable = true;
        }

        // 입력 대기 시작
        readyForInput = true;

        // "Press any key" 깜빡임
        if (pressAnyKeyText != null)
            blinkRoutine = StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        const float interval = 0.6f;
        while (true)
        {
            pressAnyKeyText.enabled = true;
            yield return new WaitForSecondsRealtime(interval);
            pressAnyKeyText.enabled = false;
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    void Update()
    {
        if (!readyForInput) return;

        bool any =
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Mouse.current != null && (
                Mouse.current.leftButton.wasPressedThisFrame ||
                Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame ||
                Mouse.current.forwardButton.wasPressedThisFrame ||
                Mouse.current.backButton.wasPressedThisFrame)) ||
            (Gamepad.current != null && Gamepad.current.allControls
                .OfType<ButtonControl>()
                .Any(b => b.wasPressedThisFrame)) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

        if (any)
            GoToTitle();
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;         // 혹시 멈춰있다면 원복
        AudioListener.pause = false; // 오디오 재개
        SceneManager.LoadScene(0);
    }
}
