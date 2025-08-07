using UnityEngine;
using UnityEngine.InputSystem;

public class SlideUI : MonoBehaviour
{
    public RectTransform panel;
    public Vector2 hideOffset;
    public float speed = 10f;

    private Vector2 originalPos;
    private Vector2 targetPos;
    private bool isTabHeld = false;

    InputAction tabAction;

    void Awake()
    {
        // 원래 위치 저장 (씬에서 설정한 anchoredPosition)
        originalPos = panel.anchoredPosition;
        targetPos = originalPos;

        // 탭키 입력 설정 (Input System)
        tabAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/tab");
        tabAction.performed += ctx => isTabHeld = true;
        tabAction.canceled += ctx => isTabHeld = false;
        tabAction.Enable();
    }

    void OnDestroy()
    {
        tabAction.Disable();
    }

    void Update()
    {
        // 이동 목표 위치 설정
        targetPos = isTabHeld ? originalPos + hideOffset : originalPos;

        // 부드럽게 이동
        panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, targetPos, Time.deltaTime * speed);
    }
}
