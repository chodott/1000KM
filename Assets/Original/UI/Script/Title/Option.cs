using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Toggle vSyncToggle;
    public Button applyButton;

    // 대표 해상도 목록
    private readonly Vector2Int[] resolutions =
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440),
        new Vector2Int(3840, 2160)
    };

    void Start()
    {
        // 드롭다운 옵션 채우기
        resolutionDropdown.ClearOptions();
        foreach (var r in resolutions)
            resolutionDropdown.options.Add(new Dropdown.OptionData($"{r.x} × {r.y}"));
        resolutionDropdown.value = 1; // 기본 1920x1080 선택
        resolutionDropdown.RefreshShownValue();

        // 현재 상태 반영
        fullscreenToggle.isOn = Screen.fullScreen;
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;

        applyButton.onClick.AddListener(Apply);
    }

    void Apply()
    {
        var sel = resolutions[resolutionDropdown.value];

        // VSync 적용
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;

        // 해상도 & 전체화면 적용
        Screen.SetResolution(sel.x, sel.y, fullscreenToggle.isOn);

    }
}
