using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;       // PausePanel (비활성 시작)
    [SerializeField] Text countdownText;          // UI Text(카운트다운 표시용, 처음엔 비활성화)

    bool paused = false;
    float prevTimeScale = 1f;
    InputAction pauseAction;

    void Awake()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        pauseAction = new InputAction("Pause", binding: "<Keyboard>/escape");
        pauseAction.performed += _ => TogglePause();
        pauseAction.Enable();
    }

    void OnDisable()
    {
        pauseAction.Disable();
    }

    void TogglePause()
    {
        if (paused) Resume();
        else Pause();
    }

    void Pause()
    {
        paused = true;

        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void Resume()
    {
        if (!paused) return;
        paused = false;

        // 패널 닫기
        if (pausePanel != null) pausePanel.SetActive(false);

        // 카운트다운 시작
        StartCoroutine(CountdownResume());
    }

    IEnumerator CountdownResume()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            // 3 → 2 → 1 표시
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f); // unscaled 시간
            }

            countdownText.gameObject.SetActive(false);
        }

        // 게임 재개
        Time.timeScale = Mathf.Approximately(prevTimeScale, 0f) ? 1f : prevTimeScale;
        AudioListener.pause = false;
    }

    public void QuitToTitle()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }
}
