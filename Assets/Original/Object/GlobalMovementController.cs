using AmazingAssets.CurvedWorld;
using System;
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
    private float _bossInterval = 1000f;
    [SerializeField]
    private float _restAreaInterval = 500f;


    private PlayerMovement _playerMovement;
    private float _distanceThreshold = 500f;
    private float _distanceAccumulator = 0f;
    private float _restAreaSpawnDistance = 200f;
    private float _bossSpawnDistance;



    //(Horizontal, Vertical)
    private (float, float) _targetBendSize = (3,1);
    private (float, float) _curBendSize = (3,1);


    public static GlobalMovementController Instance { get; private set; }
    public event Action OnReachedMaxDistance;
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
        _bossSpawnDistance += _bossInterval;
    }

    private void OnDestroy()
    {
        _playerMovement.OnSpeedChanged -= HandleVelocityChanged;
    }

    private void FixedUpdate()
    {
        float distance = GlobalVelocity * Time.fixedDeltaTime;
        TotalDistance += distance;
        _distanceAccumulator += distance;

        if(TotalDistance >= _restAreaSpawnDistance)
        {
            _restAreaEntrance.gameObject.SetActive(true);
            _restAreaSpawnDistance += _restAreaInterval;
        }

        if(TotalDistance >= _bossSpawnDistance)
        {
            OnReachedMaxDistance?.Invoke();
            _bossSpawnDistance += _bossInterval;
        }

        float lerpAlpha = _distanceAccumulator / _distanceThreshold;
        _curBendSize.Item1 = Mathf.Lerp(_curBendSize.Item1, _targetBendSize.Item1, lerpAlpha);
        _curBendSize.Item2 = Mathf.Lerp(_curBendSize.Item2, _targetBendSize.Item2, lerpAlpha);
        _curvedWorldController.bendHorizontalSize = _curBendSize.Item1;
        _curvedWorldController.bendVerticalSize = _curBendSize.Item2;

        if (_distanceAccumulator >= _distanceThreshold)
        {
            SetRoadRandomBend();
            _distanceAccumulator = 0f;
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
        _targetBendSize.Item1 = UnityEngine.Random.Range(-1f, 1f) * _maxBendSize;
        _targetBendSize.Item2 = UnityEngine.Random.Range(-1f, 1f) + _maxBendSize;
    }
}
