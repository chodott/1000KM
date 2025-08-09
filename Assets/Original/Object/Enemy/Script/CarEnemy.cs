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
    private float _curVelocity;

    private float _friction = 1f;
    private float _knockbackPower = 0.5f;

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
        Destroyed,
        NotUse
    }

    void Update()
    {
        if(_enemyState == EnemyState.NotUse)
        {
            return;
        }

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
                MoveForward(_statData.Velocity);
                CheckPattern();
                break;

             case EnemyState.Knockback:
                CheckKnockback();
                break;

             case EnemyState.Destroyed:
                MoveForward(0f);
                break;
        }

        
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
        float curVelocity = _curVelocity - GlobalMovementController.Instance.globalVelocity;
        Vector3 targetPosition = _rigidbody.position + (_forwardVector * curVelocity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(targetPosition);

        _curVelocity -= _friction * Time.fixedDeltaTime * _curVelocity;
        if(_curVelocity <= _statData.Velocity)
        {
            _curVelocity = _statData.Velocity;
            _enemyState = EnemyState.Drive;
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

    public void Init(EnemyColor color, EnemyStatData statData, Vector3 position, int laneIndex)
    {
        gameObject.SetActive(true);

        _statData = statData;
        _curVelocity = statData.Velocity;
        _healthPoint = statData.HealthPoint;
        _color = color;
        _meshFilter.mesh = statData.Mesh;
        _meshRenderer.material = _statData.materialVariants[(int)color];
        transform.localScale = statData.Scale;
        _collider.size = statData.ColliderSize;
        _collider.center = statData.ColliderCenter;

        transform.position = position;
        transform.rotation = statData.SpawnRotation;
        _rigidbody.WakeUp();

        _enemyState = EnemyState.Drive;
        _laneMover.Init(laneIndex);

    }
    public void OnParried(Vector3 direction, float damage)
    {

        _healthPoint -= damage;
        if (_healthPoint <= 0f)
        {   //Destroy
            _rigidbody.useGravity = true;
            _rigidbody.AddTorque(direction * 30f, ForceMode.Impulse);
            _enemyState = EnemyState.Destroyed;
        }
        else
        {   //Knockback
            _curVelocity = _statData.Velocity + (GlobalMovementController.Instance.globalVelocity * (1+_knockbackPower));
            _enemyState = EnemyState.Knockback;

        }
    }

    #region PoolingObject Callbacks
    public void Activate(GameObject originalPrefab)
    {
        _originalPrefab = originalPrefab;
        _rigidbody.Sleep();

    }

    public void Deactivate()
    {
        _enemyState = EnemyState.NotUse;
        _rigidbody.Sleep();
        OnReturned?.Invoke(OriginalPrefab,gameObject);
    }
    #endregion
}
