using UnityEngine;

public class CarEnemy : MonoBehaviour,IParryable, IPoolingObject
{

    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private LaneMover _laneMover;
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private BoxCollider _collider;


    private EnemyColor _color;
    private EnemyStatData _statData;
    private GameObject _originalPrefab;
    private float _patternCooldownTime = 1;
    private float _patternCooldownTimer;
    private bool _isParried;

    public GameObject OriginalPrefab { get { return _originalPrefab; }}

    public event System.Action<GameObject, GameObject> OnReturned;

    public enum EnemyColor
    {
        White,
        Grey,
        Black,
        Yellow
    }

    void Update()
    {
        if (_isParried == false)
        {
            float curVelocity = _statData.Velocity - GlobalMovementController.Instance.globalVelocity ;
            transform.position += (transform.forward * curVelocity * Time.deltaTime);

            if (_patternCooldownTimer > _patternCooldownTime)
            {
                DoPattern();
                _patternCooldownTimer = 0;
            }
            else
            {
                _patternCooldownTimer += Time.deltaTime;
            }
        }
    }

    private void DoPattern()
    {
        int direction = 1;
        switch(_color)
        {
            case EnemyColor.White:
                direction = 0;
                break;

             case EnemyColor.Grey:
                direction = -1;
                break;

             case EnemyColor.Black:
                direction = 1;
                break;
            case EnemyColor.Yellow:
                direction = Random.Range(0, 3) - 1;
                break;
        }

        _laneMover.MoveLane(direction);

    }

    public void Init(EnemyColor color, EnemyStatData statData, int laneIndex)
    {
        _statData = statData;
        _color = color;
        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];
        _collider.size = statData.ColliderSize;
        _collider.center = statData.ColliderCenter;
        _laneMover.Init(laneIndex);
    }
    public void OnParried(Vector3 force)
    {
        Deactivate();
        return;

        _isParried = true;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddTorque(force, ForceMode.Impulse);
    }

    #region PoolingObject Callbacks
    public void Activate(GameObject originalPrefab)
    {
        _originalPrefab = originalPrefab;
    }

    public void Deactivate()
    {
        OnReturned?.Invoke(OriginalPrefab,gameObject);
    }
    #endregion
}
