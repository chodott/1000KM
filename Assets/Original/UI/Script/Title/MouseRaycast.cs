using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycast : MonoBehaviour
{
    Camera cam;
    TitleObj lastHitObj;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null)
        {
            HandleChange(null);
            return;
        }

        Vector2 screenPos = Mouse.current.position.ReadValue();

        // 카메라 뷰 밖이면 맞은 오브젝트 없음으로 처리
        if (!cam.pixelRect.Contains(screenPos))
        {
            HandleChange(null);
            return;
        }

        // 3D 레이캐스트
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        TitleObj hitObj = null;

        if (Physics.Raycast(ray, out hit))
            hitObj = hit.collider.GetComponent<TitleObj>();

        HandleChange(hitObj);
    }

    void HandleChange(TitleObj hitObj)
    {
        if (hitObj == lastHitObj) return;

        // 이전 오브젝트에서 나감
        if (lastHitObj != null)
            lastHitObj.MouseExit();

        // 새 오브젝트로 들어감
        if (hitObj != null)
            hitObj.MouseEnter();

        lastHitObj = hitObj;
    }

    void OnDisable()
    {
        // 스크립트 비활성화 시 깔끔하게 Exit
        if (lastHitObj != null)
        {
            lastHitObj.MouseExit();
            lastHitObj = null;
        }
    }
}
