using AmazingAssets.CurvedWorld;
using System;
using System.Collections;
using UnityEngine;

public class GlobalMovementController : MonoBehaviour
{
    [SerializeField]
    private CurvedWorldController _curvedWorldController;
    [SerializeField]
    private RestAreaEntrance _restAreaEntrance;
    [SerializeField]
    private float _maxBendSize = 6f;


    [SerializeField]
    private float _restAreaInterval = 500f;


    private PlayerMovement _playerMovement;
    private float _distanceThreshold = 2000f;
    private float _distanceAccumulator = 0f;
    private float _restAreaSpawnDistance;
    private bool _stopCheckDistance = false;
    private bool _restActive = false;

    //(Horizontal, Vertical)
    private Vector2 _targetBendSize = new Vector2(1, 1);
    private Vector2 _prevBendSize = new Vector2(0, 0);
    private Vector2 _curBendSize;


    public static GlobalMovementController Instance { get; private set; }
    public float GlobalVelocity { get; private set; }
    public float TotalDistance { get; private set; }


    #region Monobehaviour Callbacks
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnPhaseChanged += OnPhaseChanged;
        _restAreaEntrance.OnDeactivated += OnRestDeactivated;
        SetRoadRandomBend();
        _restAreaSpawnDistance += _restAreaInterval;
    }

    private void OnDestroy()
    {
        _playerMovement.OnSpeedChanged -= HandleVelocityChanged;
        _restAreaEntrance.OnDeactivated -= OnRestDeactivated;

    }

    private void FixedUpdate()
    {

        if (_stopCheckDistance)
        {
            return;
        }

        float distance = GlobalVelocity * Time.fixedDeltaTime;
        TotalDistance += distance;
        _distanceAccumulator += distance;

        if (TotalDistance >= _restAreaSpawnDistance + _restAreaEntrance.SpawnPosX)
        {
            if(_restActive == false)
            {
                _restActive = true;
                _restAreaEntrance.Activate();
            }
            else if(_restActive && TotalDistance >= _restAreaSpawnDistance)
            {
                _restAreaSpawnDistance += _restAreaInterval;
            }
        }

        float lerpAlpha = _distanceAccumulator / _distanceThreshold;
        float curHorizontalBend = Mathf.Lerp(_prevBendSize.x, _targetBendSize.x, lerpAlpha);
        float curVerticalBend = Mathf.Lerp(_prevBendSize.y, _targetBendSize.y, lerpAlpha);
        _curBendSize = new Vector2(curHorizontalBend, curVerticalBend);
        ApplyBend(_curBendSize);

        if (_distanceAccumulator >= _distanceThreshold)
        {
            ResetTargetBend();
        }
    }
    #endregion

    public void Init(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
        _playerMovement.OnSpeedChanged += HandleVelocityChanged;
    }

    private void HandleVelocityChanged(float velocity)
    {
        GlobalVelocity = velocity;
    }

    private void SetRoadRandomBend()
    {
        _targetBendSize.x = UnityEngine.Random.Range(-1f, 1f) * _maxBendSize;
        _targetBendSize.y = UnityEngine.Random.Range(-1f, 1f) * _maxBendSize;
    }

    private void ApplyBend(Vector2 curBendSize)
    {
        _curvedWorldController.bendHorizontalSize = curBendSize.x;
        _curvedWorldController.bendVerticalSize = curBendSize.y;
    }

    private void OnRestDeactivated()
    {
        { _restActive = false; };
    }

    private void OnPhaseChanged(GamePhase phase, PhaseData data)
    {
        switch (phase)
        {
            case GamePhase.BossIntro:
                LockBendSize(data.mapBendSize, data.duration);
                break;

            case GamePhase.Normal:
                UnlockBendSize();
                break;
        }
    }

    private void ResetTargetBend()
    {
        _prevBendSize = _curBendSize;
        _distanceAccumulator = 0f;

        SetRoadRandomBend();
    }

    private void UnlockBendSize()
    {
        ResetTargetBend();
        _stopCheckDistance = false;
    }

    IEnumerator LerpLockBend(Vector2 from, Vector2 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            float u = t / duration;
            u = u * u * (3f - 2f * u);

            float x = Mathf.Lerp(from.x, to.x, u);
            float y = Mathf.Lerp(from.y, to.y, u);
            _curBendSize = new Vector2(x, y);

            ApplyBend(_curBendSize);
            t += Time.deltaTime;
            yield return null;
        }

        _curBendSize = to;
        ApplyBend(to);
    }

    public void LockBendSize(Vector2 bendSize, float duration)
    {
        _stopCheckDistance = true;
        _targetBendSize = bendSize;
        StartCoroutine(LerpLockBend(_curBendSize, bendSize, duration));
    }

    public float GetDistanceToRest()
    {
        float distance = _restAreaSpawnDistance - TotalDistance;
        return distance;
    }

}
