using Unity.VisualScripting;
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
    private float _healthPoint;

    private float _knockbackEndVelocity = 0f;
    private float _knockbackTimer = 0f;
    private float _knockbackTime = 0.2f;
    private float _velocitySmoothless = 0.3f;
    private bool _isParried;
    private bool _isKnockback;

    public GameObject OriginalPrefab { get { return _originalPrefab; }}

    public event System.Action<GameObject, GameObject> OnReturned;

    public enum EnemyColor
    {
        White,
        Grey,
        Black,
        Yellow
    }

    void FixedUpdate()
    {
        if (_isParried == false)
        {
            float velocity = _statData.Velocity;
            if(_knockbackEndVelocity > velocity)
            {
                velocity = Mathf.Lerp(_knockbackEndVelocity, _statData.Velocity, _velocitySmoothless * Time.fixedDeltaTime);
                float gap = Mathf.Abs(velocity - _statData.Velocity);
                velocity = gap <= 0.05f ? _statData.Velocity : velocity;
            }

            float curVelocity = velocity - GlobalMovementController.Instance.globalVelocity ;
            Vector3 targetPosition = transform.position + (transform.forward * curVelocity * Time.fixedDeltaTime);
            _rigidbody.MovePosition(targetPosition);

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
        else
        {
            CheckKnockback();
        }
    }

    private void CheckKnockback()
    {
        if (_isKnockback == false)
        {
            float curVelocity = _rigidbody.linearVelocity.magnitude;
            if (curVelocity <= 0.1f)
            {
                _rigidbody.linearDamping = 0f;
                _isParried = false;
                _knockbackEndVelocity = GlobalMovementController.Instance.globalVelocity;
            }
        }
        else
        {
            _knockbackTimer += Time.fixedDeltaTime;
            if (_knockbackTimer > +_knockbackTime)
            {
                _isKnockback = false;
                _knockbackTimer = 0;
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
        _healthPoint = statData.HealthPoint;
        _color = color;
        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];
        _collider.size = statData.ColliderSize;
        _collider.center = statData.ColliderCenter;
        _laneMover.Init(laneIndex);
    }
    public void OnParried(Vector3 direction, float force, float damage)
    {

        _healthPoint -= damage;
        _isParried = true;

        if (_healthPoint <= 0f)
        {
            Deactivate();
            return;

            _rigidbody.useGravity = true;
            _rigidbody.AddTorque(direction * force, ForceMode.Impulse);
        }
        else
        {   //Knockback
            _rigidbody.linearDamping = 2.0f;
            _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
            _isKnockback = true;

        }
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
