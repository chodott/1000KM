using UnityEngine;
using System.Collections.Generic;

public class FakeMotionFeeder : MonoBehaviour
{
    public Vector3 worldDirection = -Vector3.right;

    public float smoothing = 0f;
    public float _blurScale = 0.1f;

    static readonly int ShaderId_FakeLocalDelta = Shader.PropertyToID("_FakeLocalDelta");

    readonly List<Renderer> _renderers = new List<Renderer>();
    MaterialPropertyBlock _mpb;
    Vector3 _currentLocalDelta; 

    void Awake()
    {
        GetComponentsInChildren(true, _renderers);
        _mpb = new MaterialPropertyBlock();
    }

    void LateUpdate()
    {
        float curVelocity = GlobalMovementController.Instance.GlobalVelocity;
        Vector3 fakeWorldDelta = worldDirection * curVelocity * Time.deltaTime * _blurScale;

        Vector3 targetLocalDelta = transform.InverseTransformDirection(fakeWorldDelta);

        if (smoothing > 0f)
            _currentLocalDelta = Vector3.Lerp(_currentLocalDelta, targetLocalDelta, 1f - Mathf.Exp(-smoothing * Time.deltaTime));
        else
            _currentLocalDelta = targetLocalDelta;

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