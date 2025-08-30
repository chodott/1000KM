using System;
using System.Collections;
using UnityEngine;

public class InvincibleSystem : MonoBehaviour
{
    [SerializeField]
    private float _invincibleTime = 1.0f;

    private float _invincibleTimer = 0f;
    private bool _isActive = false;

    public event Action OnFinishedInvincible;

    public bool IsActive { get { return  _isActive; } }

    public void StartInvinble()
    {
        if(IsActive)
        {
            return;
        }
        StartCoroutine(Run());
    }


    IEnumerator Run()
    {
        _isActive = true;
        while(_invincibleTimer < _invincibleTime) 
        {
            _invincibleTimer += Time.unscaledDeltaTime;
            yield return null;
        }
        _isActive = false;
        OnFinishedInvincible?.Invoke();
    }

}
