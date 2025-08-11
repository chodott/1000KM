using UnityEngine;
using UnityEngine.InputSystem;

public class Slide3D : MonoBehaviour
{
    [Tooltip("처음 위치(복귀 지점)")]
    public Transform fromTransform;

    [Tooltip("이동할 위치(탭 누르는 동안)")]
    public Transform toTransform;

    [Tooltip("이 스크립트가 붙은 객체 대신 다른 객체를 이동시키고 싶다면 지정")]
    public Transform moveTarget;

    [Tooltip("보간 속도")]
    public float speed = 10f;

    private Vector3 fallbackFromPos; // fromTransform이 없을 때 현재 위치를 사용
    private Vector3 targetPos;
    private bool isTabHeld = false;

    private InputAction tabAction;

    void Awake()
    {
        if (moveTarget == null) moveTarget = transform;
        fallbackFromPos = moveTarget.position;

        tabAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/tab");
        tabAction.performed += _ => isTabHeld = true;
        tabAction.canceled += _ => isTabHeld = false;
        tabAction.Enable();
    }

    void OnDestroy()
    {
        if (tabAction != null) tabAction.Disable();
    }

    void Update()
    {
        // 목표 위치 선택
        Vector3 fromPos = (fromTransform != null) ? fromTransform.position : fallbackFromPos;
        Vector3 toPos = (toTransform != null) ? toTransform.position : fromPos;

        targetPos = isTabHeld ? toPos : fromPos;

        // 부드럽게 이동
        moveTarget.position = Vector3.Lerp(moveTarget.position, targetPos, Time.deltaTime * speed);
    }
}