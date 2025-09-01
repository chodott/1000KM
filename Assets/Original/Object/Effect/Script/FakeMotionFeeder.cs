using UnityEngine;
using System.Collections.Generic;

public class FakeMotionFeeder : MonoBehaviour
{
    public Vector3 worldDirection = -Vector3.right;

    [Tooltip("0이면 매 프레임 즉시 반영. 값을 올리면 델타가 서서히 따라감")]
    public float smoothing = 0f;

    static readonly int ShaderId_FakeLocalDelta = Shader.PropertyToID("_FakeLocalDelta");

    readonly List<Renderer> _renderers = new List<Renderer>();
    MaterialPropertyBlock _mpb;
    Vector3 _currentLocalDelta;  // 스무딩용 누적값

    void Awake()
    {
        GetComponentsInChildren(true, _renderers);
        _mpb = new MaterialPropertyBlock();
    }

    void LateUpdate()
    {
        float curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        Vector3 fakeWorldDelta = worldDirection * curVelocity * Time.deltaTime;

        Vector3 targetLocalDelta = transform.InverseTransformDirection(fakeWorldDelta);

        // 3) 스무딩 옵션 적용
        if (smoothing > 0f)
            _currentLocalDelta = Vector3.Lerp(_currentLocalDelta, targetLocalDelta, 1f - Mathf.Exp(-smoothing * Time.deltaTime));
        else
            _currentLocalDelta = targetLocalDelta;

        // 4) 모든 렌더러에 MPB로 주입
        foreach (var r in _renderers)
        {
            if (!r) continue;
            r.GetPropertyBlock(_mpb);
            _mpb.SetVector(ShaderId_FakeLocalDelta, _currentLocalDelta);
            r.SetPropertyBlock(_mpb);
        }
    }

    // 정지시키고 싶을 때 호출
    public void ClearMotion()
    {
        _currentLocalDelta = Vector3.zero;
        foreach (var r in _renderers)
        {
            if (!r) continue;
            r.GetPropertyBlock(_mpb);
            _mpb.SetVector(ShaderId_FakeLocalDelta, Vector4.zero);
            r.SetPropertyBlock(_mpb);
        }
    }
}