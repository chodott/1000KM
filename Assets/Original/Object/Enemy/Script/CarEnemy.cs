using System.Collections;
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


    private EnemyState _enemyState;
    private EnemyColor _color;
    private EnemyStatData _statData;
    private GameObject _originalPrefab;
    private Vector3 _forwardVector = new Vector3(-1, 0, 0);
    private float _returnPositionX = 5f;
    private float _patternCooldownTime = 1;
    private float _patternCooldownTimer;
    private float _healthPoint;

    private float _knockbackEndVelocity = 0f;
    private float _knockbackTimer = 0f;
    private float _knockbackTime = 0.2f;
    private float _velocitySmoothless = 0.3f;

    public GameObject OriginalPrefab { get { return _originalPrefab; }}

    public event System.Action<GameObject, GameObject> OnReturned;

    public enum EnemyColor
    {
        White,
        Grey,
        Black,
        Yellow
    }

    public enum EnemyState
    {
        Drive,
        Knockback,
        Parried
    }

    void Update()
    {
        if(transform.position.x >=_returnPositionX)
        {
            Deactivate();
        }
    }

    void FixedUpdate()
    {

        switch(_enemyState)
        {
            case EnemyState.Drive:
                float velocity = RecoverVelocity(_statData.Velocity);
                MoveForward(velocity);
                CheckPattern();
                break;

             case EnemyState.Knockback:
                CheckKnockback();
                break;

             case EnemyState.Parried:
                MoveForward(0f);
                break;
        }

        
    }

    private float RecoverVelocity(float velocity)
    {
        if (_knockbackEndVelocity > velocity)
        {
            velocity = Mathf.Lerp(_knockbackEndVelocity, _statData.Velocity, _velocitySmoothless * Time.fixedDeltaTime);
            float gap = Mathf.Abs(velocity - _statData.Velocity);
            velocity = (gap <= 0.05f) ? _statData.Velocity : velocity;
        }
        return velocity;
    }

    private void MoveForward(float velocity)
    {
        float curVelocity = velocity - GlobalMovementController.Instance.globalVelocity;
        Vector3 targetPosition = _rigidbody.position + (_forwardVector * curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);
    }

    private void CheckPattern()
    {
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

    private void CheckKnockback()
    {
        _knockbackTimer += Time.fixedDeltaTime;
        if (_knockbackTimer > +_knockbackTime)
        {
            _enemyState = EnemyState.Drive;
            _knockbackTimer = 0;
            float curVelocity = _rigidbody.linearVelocity.magnitude;
            if (curVelocity <= 0.1f)
            {
                _rigidbody.linearDamping = 0f;
                _knockbackEndVelocity = GlobalMovementController.Instance.globalVelocity;
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

    public void Init(EnemyColor color, EnemyStatData statData, Vector3 position, Quaternion rotation, int laneIndex)
    {
        _statData = statData;
        _healthPoint = statData.HealthPoint;
        _color = color;
        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];
        _collider.size = statData.ColliderSize;
        _collider.center = statData.ColliderCenter;

        _rigidbody.useGravity = false;
        _rigidbody.position = position;
        _rigidbody.rotation = rotation;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.linearDamping = 0f;
        _rigidbody.WakeUp();

        _enemyState = EnemyState.Drive;

        _laneMover.Init(laneIndex);
    }
    public void OnParried(Vector3 direction, float force, float damage)
    {

        _healthPoint -= damage;

        if (_healthPoint <= 0f)
        {
            _rigidbody.useGravity = true;
            _rigidbody.AddTorque(direction * force, ForceMode.Impulse);
            _enemyState = EnemyState.Parried;
        }
        else
        {   //Knockback
            _rigidbody.linearDamping = 2.0f;
            _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
            _enemyState = EnemyState.Knockback;

        }
    }

    #region PoolingObject Callbacks
    public void Activate(GameObject originalPrefab)
    {
        _originalPrefab = originalPrefab;
    }

    public void Deactivate()
    {
        _rigidbody.Sleep();
        OnReturned?.Invoke(OriginalPrefab,gameObject);
    }
    #endregion
}
