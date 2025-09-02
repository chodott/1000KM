using System.Collections;
using UnityEngine;

public class AudioHoldCrossfade : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource mainAudio;          // 원래 오디오 (A)
    public AudioSource subAudio;           // 버튼까지 유지할 오디오 (B)

    [Header("Volumes")]
    [Range(0f, 1f)] public float mainTargetVolume = 1f; // A의 평상시 볼륨
    [Range(0f, 1f)] public float subTargetVolume = 1f;  // B가 유지할 볼륨

    [Header("Durations (seconds)")]
    public float fadeDurationMainDown = 1.2f;  // A -> 0
    public float fadeDurationSubUp = 0.8f;   // B -> subTargetVolume
    public float fadeDurationSubDown = 0.8f;   // B -> 0
    public float fadeDurationMainUp = 1.2f;   // A -> mainTargetVolume

    enum State { Idle, HoldingSub }      // Idle: A 활성, HoldingSub: B 유지 중
    State _state = State.Idle;

    Coroutine _running;

    void Awake()
    {
        if (mainAudio != null) mainTargetVolume = mainAudio.volume;
        if (subAudio != null) subAudio.volume = 0f; // 시작은 0으로
    }

    /// <summary>
    /// 1단계: A를 0까지 페이드→ B를 켜서 subTargetVolume까지 페이드 후 "유지"
    /// </summary>
    public void BeginHold()
    {
        if (_state == State.HoldingSub) return; // 이미 B 유지 중이면 무시
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(BeginHoldRoutine());
    }

    /// <summary>
    /// 2단계: (버튼 호출) B를 0까지 페이드 아웃 & 정지 → A를 원래 볼륨으로 복귀
    /// </summary>
    public void ResumeFromHold()
    {
        if (_state != State.HoldingSub) return; // 유지 중이 아니면 무시
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(ResumeFromHoldRoutine());
    }

    IEnumerator BeginHoldRoutine()
    {
        // A가 없거나 B가 없으면 안전 장치
        if (mainAudio == null || subAudio == null) yield break;

        // A 페이드다운 (→0)
        yield return FadeVolume(mainAudio, mainAudio.volume, 0f, fadeDurationMainDown);

        // B 재생 및 페이드업 (0→target) 후 유지
        if (!subAudio.isPlaying) subAudio.Play();
        yield return FadeVolume(subAudio, subAudio.volume, subTargetVolume, fadeDurationSubUp);

        _state = State.HoldingSub;
        _running = null;
    }

    IEnumerator ResumeFromHoldRoutine()
    {
        if (mainAudio == null || subAudio == null) yield break;

        // B 페이드다운 (→0) 후 정지
        yield return FadeVolume(subAudio, subAudio.volume, 0f, fadeDurationSubDown);
        subAudio.Stop();
        subAudio.time = 0f; // 다음에 처음부터 재생하고 싶다면 유지

        // A 페이드업 (0→원래 볼륨)
        yield return FadeVolume(mainAudio, mainAudio.volume, mainTargetVolume, fadeDurationMainUp);

        _state = State.Idle;
        _running = null;
    }

    static IEnumerator FadeVolume(AudioSource source, float from, float to, float duration)
    {
        if (source == null) yield break;

        float t = 0f;
        source.volume = from;

        // duration이 0이면 즉시 전환
        if (duration <= 0f)
        {
            source.volume = to;
            yield break;
        }

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // 게임 일시정지 대비: 필요시 Time.deltaTime으로 바꾸세요
            float k = Mathf.Clamp01(t / duration);
            source.volume = Mathf.Lerp(from, to, k);
            yield return null;
        }
        source.volume = to;
    }
}